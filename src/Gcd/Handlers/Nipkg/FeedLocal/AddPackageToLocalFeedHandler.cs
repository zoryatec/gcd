using CSharpFunctionalExtensions;
using Gcd.Services;
using Gcd.Model;
using MediatR;
using Gcd.Model.Config;
using System.IO.Compression;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.Common;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Handlers.Nipkg.FeedLocal;

public static class MediatorExtensions
{
    public static async Task<Result> AddToLocalFeedAsync(this IMediator mediator,
        FeedDefinitionLocal localFeedDefinition,
        IPackageFileDescriptor packageFileDescriptor,
        NipkgCmdPath CmdPath,
        UseAbsolutePath useAbsolutePath,
        bool createFeed = false,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new AddPackageToLocalRequest(
            localFeedDefinition, 
            new List<IPackageFileDescriptor>{packageFileDescriptor}.AsReadOnly(),
            CmdPath, createFeed,
            useAbsolutePath), 
            cancellationToken);
    
    public static async Task<Result> AddToLocalFeedAsync(this IMediator mediator,
        FeedDefinitionLocal localFeedDefinition,
        IReadOnlyList<IPackageFileDescriptor> packageFileDescriptors,
        NipkgCmdPath CmdPath,
        UseAbsolutePath useAbsolutePath,
        bool createFeed = false,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new AddPackageToLocalRequest(localFeedDefinition, packageFileDescriptors, CmdPath, createFeed, useAbsolutePath), cancellationToken);
}



public record UseAbsolutePath
{
    private string _value;
    public UseAbsolutePath(string value) { _value = value; }

    public static UseAbsolutePath Yes = new UseAbsolutePath("yes");
    public static UseAbsolutePath No = new UseAbsolutePath("no");
}

public record AddPackageToLocalRequest(FeedDefinitionLocal AzFeedDef, IReadOnlyList<IPackageFileDescriptor> packagePaths, NipkgCmdPath CmdPath, bool createFeed, UseAbsolutePath UseAbsolutePath) : IRequest<Result>;

public class AddPackageToLocalHandler(
    IMediator _mediator,
    IUploadAzBlobService _uploadService,
    IFileSystem _fs,
    IWebDownload _webDownload)
    : IRequestHandler<AddPackageToLocalRequest, Result>
{
    public async Task<Result> Handle(AddPackageToLocalRequest request, CancellationToken cancellationToken)
    {
        var (localFeedDef, packagePaths, cmdPath, createFeed, useAbsPath) = request;


        foreach (var packagePath in packagePaths)
        {
            var insideFeedPkgPath = PackageLocalFilePath.Of(localFeedDef.Feed, packagePath.FileName);

            var result = await DownloadFile(packagePath, insideFeedPkgPath, overwrite: true)
                .Bind(() => _mediator.AddPackageToLcalFeedAsync(localFeedDef, insideFeedPkgPath, cmdPath, createFeed));

            if (result.IsFailure) return result;

            if (useAbsPath.Equals(UseAbsolutePath.Yes)) return await UpdateToAbsPath(localFeedDef, packagePath);

            if (result.IsFailure) return result;
        }
        return Result.Success();
    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));



    private async Task<Result> DownloadFile(IFileDescriptor sourceDescriptor, ILocalFilePath destinationPath, bool overwrite = false)
    {
        return sourceDescriptor switch
        {
            ILocalFilePath source => await _fs.CopyFileAsync(source, destinationPath, overwrite: overwrite),
            IWebFileUri source => await _webDownload.DownloadFileAsync(source, destinationPath),
            _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
        };
    }
    private async Task<Result> UploadFile(IFileDescriptor destinationDescriptor, ILocalFilePath destinationPath)
    {
        return destinationDescriptor switch
        {
            ILocalFilePath source => await _fs.CopyFileAsync(source, destinationPath, overwrite: true),
            _ => throw new InvalidOperationException(destinationDescriptor.GetType().Name)
        };
    }

    private async Task<Result> UpdateToAbsPath(FeedDefinitionLocal localFeedDefinition, IPackageFileDescriptor descriptor)
    {
        string absoluteUri = descriptor.Value;
        var contentR = await _fs.ReadTextFileAsync(localFeedDefinition.Package);
        var content = contentR.Value.Replace(descriptor.FileName.Value, absoluteUri);
        var resultWrite = await _fs.WriteTextFileAsync(localFeedDefinition.Package, content);

        RecreateGz(localFeedDefinition);

        var stampsContentR = await _fs.ReadTextFileAsync(localFeedDefinition.PackageStamps);
        var stampsContent = stampsContentR.Value.Replace(descriptor.FileName.Value, absoluteUri);
        var resultWriteStamps = await _fs.WriteTextFileAsync(localFeedDefinition.PackageStamps, stampsContent);

        return resultWriteStamps;
    }

    private void RecreateGz(FeedDefinitionLocal localFeedDefinition)
    {
        string sourceFile = localFeedDefinition.Package.Value; // Path to the file to be compressed
        string destinationFile = localFeedDefinition.PackageGz.Value; // Path for the compressed file
        if (File.Exists(destinationFile)) File.Delete(destinationFile);


        using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
        using (FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
        using (GZipStream gzipStream = new GZipStream(destinationStream, CompressionMode.Compress))
        {
            sourceStream.CopyTo(gzipStream);
        }

    }

    private async Task<Result> UploadPackage(FeedDefinitionAzBlob azFeedDef, PackageLocalFilePath packagePath)
    {
        var azblob = AzBlobFeedUri.Create(azFeedDef.Feed.Full); ;
        string nipkgUrl = CreateSubUrl(azblob.Value, packagePath.FileName.Value);

        var blobUri = AzBlobUri.Create(nipkgUrl);
        var result = await _uploadService.UploadFileAsync(blobUri.Value, packagePath);
        return result;
    }

    private string CreateSubUrl(AzBlobFeedUri feedUri, string subPath) =>
        $"{feedUri.BaseUri}/{subPath}{feedUri.Query}";

}
