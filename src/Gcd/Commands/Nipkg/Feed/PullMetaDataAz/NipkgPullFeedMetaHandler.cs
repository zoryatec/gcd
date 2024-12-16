using CSharpFunctionalExtensions;
using Gcd.Model;
using Gcd.Model.File;
using Gcd.Services;
using MediatR;

namespace Gcd.Commands.Nipkg.Feed.PullMetaDataAz;

public record NipkgPullFeedMetaRequest(AzBlobFeedDefinition AzFeedDef, LocalFeedDefinition LocalFeedDef) : IRequest<Result>;

public class NipkgPullFeedMetaHandler(IDownloadAzBlobService _ds, IFileSystem _fs)
    : IRequestHandler<NipkgPullFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPullFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => _ds.DownloadFileAsync(azFeedDef.Package, localFeedDef.Package))
            .Bind(() => _ds.DownloadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _ds.DownloadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> PullAzBlobFeedMetaDataAsync(this IMediator mediator, AzBlobFeedDefinition AzFeedDef, LocalFeedDefinition LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new NipkgPullFeedMetaRequest(AzFeedDef, LocalFeedDef), cancellationToken);
}
