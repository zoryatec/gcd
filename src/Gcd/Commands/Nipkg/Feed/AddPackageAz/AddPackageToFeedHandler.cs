using CSharpFunctionalExtensions;
using Gcd.Services;
using Gcd.Model;
using MediatR;
using Gcd.Commands.Nipkg.Feed.PullMetaDataAz;
using Gcd.Commands.Nipkg.Feed.PushMetaDataAz;
using Gcd.Model.Config;
using Gcd.Services.FileSystem;
using System.IO.Compression;

namespace Gcd.Commands.Nipkg.Feed.AddPackageAz;

public record AddPackageToFeedRequest(AzBlobFeedDefinition AzFeedDef, PackageFilePath PackagePath, NipkgCmdPath CmdPath) : IRequest<Result>;
public record AddPackageToFeedResponse(string Result);

public class AddPackageToFeedHandler(
    IMediator _mediator,
    IUploadAzBlobService _uploadService,
    IFileSystem _fs)
    : IRequestHandler<AddPackageToFeedRequest, Result>
{
    public async Task<Result> Handle(AddPackageToFeedRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, packagePath, cmdPath) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = localFeedDef
            .Map((arg) => PackageFilePath.Of(arg.Feed, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => _mediator.PullAzBlobFeedMetaDataAsync(azFeedDef, localFeedDef.Value))
            .Bind(() => _fs.CopyFileAsync(packagePath, insideFeedPkgPath.Value, true))
            .Bind(() => _mediator.AddPackageToLcalFeedAsync(localFeedDef.Value, insideFeedPkgPath.Value, cmdPath))
            .Bind(() => UpdateToAbsPath(localFeedDef.Value, azFeedDef,packagePath.FileName))
            .Bind(() => _mediator.PushAzBlobFeedMetaDataAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => UploadPackage(azFeedDef, insideFeedPkgPath.Value));
    }

    private async Task<Result<LocalFeedDefinition>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => LocalFeedDefinition.Of(feedPath));


    private async Task<Result> UpdateToAbsPath(LocalFeedDefinition localFeedDefinition, AzBlobFeedDefinition azFeedDefinition, PackageFileName packageFileName)
    {
        string absoluteUri = $"{azFeedDefinition.Feed.BaseUri}/{packageFileName.Value}";
        var contentR = await _fs.ReadTextFileAsync(localFeedDefinition.Package);
        var content = contentR.Value.Replace(packageFileName.Value, absoluteUri);
        var resultWrite =  await _fs.WriteTextFileAsync(localFeedDefinition.Package, content);

        RecreateGz(localFeedDefinition);

        var stampsContentR = await _fs.ReadTextFileAsync(localFeedDefinition.PackageStamps);
        var stampsContent = stampsContentR.Value.Replace(packageFileName.Value, absoluteUri);
        var resultWriteStamps = await _fs.WriteTextFileAsync(localFeedDefinition.PackageStamps, stampsContent);

        return resultWriteStamps;
    }

    private void RecreateGz(LocalFeedDefinition localFeedDefinition)
    {
        string sourceFile = localFeedDefinition.Package.Value; // Path to the file to be compressed
        string destinationFile = localFeedDefinition.PackageGz.Value; // Path for the compressed file
        if(File.Exists(destinationFile)) File.Delete(destinationFile);


        using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
        using (FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
        using (GZipStream gzipStream = new GZipStream(destinationStream, CompressionMode.Compress))
        {
            sourceStream.CopyTo(gzipStream);
        }
        
    }

    private async Task<Result> UploadPackage(AzBlobFeedDefinition azFeedDef, PackageFilePath packagePath)
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



