using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using LibGit2Sharp;
using MediatR;


namespace Gcd.Handlers.Nipkg.FeedGit;

public class PushFeedMetaHandler(IFileSystem _fs, IRemoteFileSystemGit rfs)
    : IRequestHandler<PushFeedMetaRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(PushFeedMetaRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;

        var result = await rfs.UploadFileAsync(localFeedDef.Package, new RelativeFilePath(RelativeDirPath.Root,localFeedDef.Package.FileName),remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName, remoteFeedDef.Password,remoteFeedDef.CommitterName, remoteFeedDef.CommitterEmail)
            .Bind(() => rfs.UploadFileAsync(localFeedDef.PackageGz,new RelativeFilePath(RelativeDirPath.Root,localFeedDef.PackageGz.FileName),remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName, remoteFeedDef.Password,remoteFeedDef.CommitterName, remoteFeedDef.CommitterEmail))
            .Bind(() => rfs.UploadFileAsync(localFeedDef.PackageStamps,new RelativeFilePath(RelativeDirPath.Root,localFeedDef.PackageStamps.FileName), remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName, remoteFeedDef.Password,remoteFeedDef.CommitterName, remoteFeedDef.CommitterEmail));
        return result.MapError(x => x.Message);
    }
}