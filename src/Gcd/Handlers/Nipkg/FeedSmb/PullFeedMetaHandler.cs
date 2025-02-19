using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedSmb;


public class PullFeedMetaHandler(IFileSystem _fs, IRemoteFileSystemSmb _rfs)
    : IRequestHandler<PullFeedMetaRequest<FeedDefinitionSmb>, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest<FeedDefinitionSmb> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;
        return await _fs.CreateDirAsync(localFeedDef.Feed)
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDef.Feed, remoteFeedDef.Package, localFeedDef.Package, remoteFeedDef.SmbUserName, remoteFeedDef.SmbPassword))
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDef.Feed, remoteFeedDef.PackageGz, localFeedDef.PackageGz, remoteFeedDef.SmbUserName, remoteFeedDef.SmbPassword))
            .Bind(() => _rfs.DownloadFileAsync(remoteFeedDef.Feed, remoteFeedDef.PackageStamps, localFeedDef.PackageStamps, remoteFeedDef.SmbUserName, remoteFeedDef.SmbPassword));
    }
}