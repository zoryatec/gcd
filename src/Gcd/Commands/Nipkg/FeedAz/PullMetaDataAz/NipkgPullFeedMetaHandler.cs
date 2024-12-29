using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Commands.Nipkg.Feed.PullMetaDataAz;

public record NipkgPullFeedMetaRequest(IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef) : IRequest<Result>;

public class NipkgPullFeedMetaHandler(IFileSystem _fs, IRemoteFileSystem _rfs)
    : IRequestHandler<NipkgPullFeedMetaRequest, Result>
{
    public async Task<Result> Handle(NipkgPullFeedMetaRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.Package, localFeedDef.Package))
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.DownloadFileAsync(azFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> PullFeedMetaDataAsync(this IMediator mediator, IFeedDefinition FeedDefinition, FeedDefinitionLocal LocalFeedDef, CancellationToken cancellationToken = default)
        => await mediator.Send(new NipkgPullFeedMetaRequest(FeedDefinition, LocalFeedDef), cancellationToken);
}
