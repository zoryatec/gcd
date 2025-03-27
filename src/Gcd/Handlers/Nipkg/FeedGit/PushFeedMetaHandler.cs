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
        
        
        var checkoutDir = await _fs.GenerateTempDirectoryAsync();
        var checkoutFeed = FeedDefinitionLocal.Of(checkoutDir.Value);

        
        
        return await checkoutFeed
        .Bind((x) => rfs.Clone(remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName, remoteFeedDef.Password, checkoutFeed.Value.Feed))
        .Bind(() => _fs.CopyFileAsync(localFeedDef.Package, checkoutFeed.Value.Package, overwrite: true))
        .Bind(() => _fs.CopyFileAsync( localFeedDef.PackageGz, checkoutFeed.Value.PackageGz, overwrite: true))
        .Bind(() => _fs.CopyFileAsync(localFeedDef.PackageStamps,checkoutFeed.Value.PackageStamps, overwrite: true))
        .Bind(() => rfs.Push(remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName, remoteFeedDef.Password, remoteFeedDef.CommitterName, remoteFeedDef.CommitterEmail, checkoutFeed.Value.Feed));
    }

    private async Task<Result> PushFeed(FeedDefinitionGit feedDefinition, LocalDirPath checkoutPath) =>
   Result.Try(() => CommitAndPush(feedDefinition, checkoutPath)).MapError(errr => $"PUSH:{errr}");


    public void CommitAndPush(FeedDefinitionGit feedDefinition, LocalDirPath checkoutPath)
    {
        var (address, branch, username, password, committerName, committerEmail) = feedDefinition;
        using (var repo = new Repository(checkoutPath.Value))
        {
            LibGit2Sharp.Commands.Stage(repo, "*");

            var status = repo.RetrieveStatus();
            if (status.IsDirty)
            {
                var author = new Signature(committerName.Value, committerEmail.Value, DateTime.Now);
                var committer = author;
                repo.Commit("gcd-commit", author, committer);
            }
            else
            {
                throw new Exception("No changes to commit."); // replace with result
            }

            var pushOptions = new PushOptions
            {
                CredentialsProvider = (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials
                    {
                        Username = username.Value,
                        Password = password.Value
                    }
            };

            repo.Network.Push(repo.Branches[branch.Value], pushOptions);
        }
    }
}