using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services;
using Gcd.Services.FileSystem;
using MediatR;

namespace Gcd.Commands.Nipkg.Feed.PullMetaDataAz;

public record NipkgPullFeedMetaRequest(IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef) : IRequest<Result>;

public class NipkgPullFeedMetaHandler(IDownloadAzBlobService _ds, IFileSystem _fs, IWebDownload _webDownload)
    : IRequestHandler<NipkgPullFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPullFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => DownloadFileAsync(azFeedDef.Package, localFeedDef.Package))
            .Bind(() => DownloadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => DownloadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
    private async Task<Result> DownloadFileAsync(IFileDescriptor sourceDescriptor, LocalFilePath destinationPath, bool overwrite = false)
    {
        return sourceDescriptor switch
        {
            LocalFilePath source => await _fs.CopyFileAsync(source, destinationPath, overwrite: overwrite),
            WebUri source => await _webDownload.DownloadFileAsync(source, destinationPath),
            AzBlobUri source => await _ds.DownloadFileAsync(source,destinationPath),
            _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
        };
    }
}



public static class MediatorExtensions
{
    public static async Task<Result> PullAzBlobFeedMetaDataAsync(this IMediator mediator, IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new NipkgPullFeedMetaRequest(FeedDefinition, LocalFeedDef), cancellationToken);
}
