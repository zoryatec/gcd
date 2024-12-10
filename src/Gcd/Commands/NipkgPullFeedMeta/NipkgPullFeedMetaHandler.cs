using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record NipkgPullFeedMetaRequest(AzBlobFeedDefinition AzFeedDef, LocalFeedDefinition LocalFeedDef) : IRequest<Result>;

public class NipkgPullFeedMetaHandler(IDownloadAzBlobService downloadService)
    : IRequestHandler<NipkgPullFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPullFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await CreateDirAsync(localFeedDef.Feed)
            .Bind(() => DownloadFileAsync(azFeedDef.Package, localFeedDef.Package))
            .Bind(() => DownloadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => DownloadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }

    private async Task<Result> CreateDirAsync(LocalDirPath locDirPath)
    {
        if (!Directory.Exists(locDirPath.Value)) Directory.CreateDirectory(locDirPath.Value);
        return Result.Success();
    }

    private async Task<Result> DownloadFileAsync(AzBlobUri uri, LocalFilePath filePath) =>
         await downloadService.DownloadFileAsync(uri, filePath);

}

public static class MediatorExtensions
{
    public static async Task<Result> PullAzBlobFeedMetaDataAsync(this IMediator mediator, AzBlobFeedDefinition AzFeedDef, LocalFeedDefinition LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new NipkgPullFeedMetaRequest(AzFeedDef, LocalFeedDef), cancellationToken);
}
