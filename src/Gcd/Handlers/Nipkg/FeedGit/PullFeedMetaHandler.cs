using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using LibGit2Sharp;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGit;


public class PullFeedMetaHandler(IFileSystem _fs, IRemoteFileSystemGit rfs)
    : IRequestHandler<PullFeedMetaRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest<FeedDefinitionGit> request,
        CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;

        // IRelativeFilePath dd = ;
        var result1 = await _fs.CreateDirAsync(localFeedDef.Feed);
        if (result1.IsFailure) return result1;
        
        var result = await rfs
            .DownloadFileAsync(new RelativeFilePath(RelativeDirPath.Root, localFeedDef.Package.FileName),
                localFeedDef.Package, remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName,
                remoteFeedDef.Password)
            .Bind(() => rfs.DownloadFileAsync(
                new RelativeFilePath(RelativeDirPath.Root, localFeedDef.PackageGz.FileName), localFeedDef.PackageGz,
                remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName, remoteFeedDef.Password))
            .Bind(() => rfs.DownloadFileAsync(
                new RelativeFilePath(RelativeDirPath.Root, localFeedDef.PackageStamps.FileName),
                localFeedDef.PackageStamps, remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName,
                remoteFeedDef.Password));
        return result.MapError(x => x.Message);
    }
}