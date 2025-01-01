using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.RemoteFeed;

public record PullFeedMetaRequest(IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef) : IRequest<Result>;

public class PullFeedMetaHandler(IFileSystem _fs, IRemoteFileSystem _rfs)
    : IRequestHandler<PullFeedMetaRequest, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.Package, localFeedDef.Package))
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}


public static class MediatorExtensionsPull
{
    public static async Task<Result> PullFeedMetaAsync(this IMediator mediator, IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new PullFeedMetaRequest(FeedDefinition, LocalFeedDef), cancellationToken);
}
