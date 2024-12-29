using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services;
using Gcd.Services.FileSystem;
using MediatR;


namespace Gcd.Commands.Nipkg.Feed.PushMetaDataAz;

public record NipkgPushAzBlobFeedMetaRequest(IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDefinition) : IRequest<Result>;
public record NipkgPushAzBlobFeedMetaRespons(string Result);

public class NipkgPushAzBlobFeedMetaHandler(IUploadAzBlobService uploadService, IFileSystem _fs)
    : IRequestHandler<NipkgPushAzBlobFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPushAzBlobFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await UploadFileAsync(azFeedDef.Package, localFeedDef.Package)
            .Bind(() => UploadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => UploadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }

    //private async Task<Result> UploadFileAsync(AzBlobUri uri, LocalFilePath filePath) =>
    // await uploadService.UploadFileAsync(uri, filePath);

    private async Task<Result> UploadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath sourcePath, bool overwrite = false)
    {
        return sourceDescriptor switch
        {
            LocalFilePath source => await _fs.CopyFileAsync(source, sourcePath, overwrite: overwrite),
            AzBlobUri source => await uploadService.UploadFileAsync(source, sourcePath),
            _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
        };
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> PushAzBlobFeedMetaDataAsync(this IMediator mediator, IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new NipkgPushAzBlobFeedMetaRequest(FeedDefinition, LocalFeedDef), cancellationToken);
}