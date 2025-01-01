using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using LibGit2Sharp;
using MediatR;


namespace Gcd.Handlers.Nipkg.FeedGit;

public class PushFeedMetaHandler(IFileSystem _fs, RemoteFileSystemGit _rfs)
    : IRequestHandler<PushFeedMetaRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(PushFeedMetaRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;
        return await PushFeed(remoteFeedDef, localFeedDef.Feed);
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