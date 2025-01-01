using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.RemoteFeed;
using Gcd.Model.FeedDefinition;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedSmb;


public class PullFeedMetaHandler(IFileSystem _fs, RemoteFileSystemSmb _rfs)
    : IRequestHandler<PullFeedMetaRequest<FeedDefinitionSmb>, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest<FeedDefinitionSmb> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDef.Package, localFeedDef.Package))
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDef.PackageGz, localFeedDef.PackageGz))
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDef.PackageStamps, localFeedDef.PackageStamps));
    }
}