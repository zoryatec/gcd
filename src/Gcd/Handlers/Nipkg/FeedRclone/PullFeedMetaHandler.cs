using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.RemoteFileSystem.Rclone.Abstractions;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedRclone;

public class PullFeedMetaHandler(IFileSystem _fs, IRemoteFileSystemRclone _rfs)
    : IRequestHandler<PullFeedMetaRequest<FeedDefinitionRclone>, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest<FeedDefinitionRclone> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDefinition, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDefinition.Package, localFeedDef.Package))
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDefinition.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDefinition.PackageStamps, localFeedDef.PackageStamps));
    }
}