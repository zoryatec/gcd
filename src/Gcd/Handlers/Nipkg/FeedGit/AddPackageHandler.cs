using CSharpFunctionalExtensions;
using Gcd.Services;
using Gcd.Model;
using MediatR;
using Gcd.Model.Config;
using Gcd.Services.FileSystem;
using Gcd.Model.File;
using Gcd.Model.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using LibGit2Sharp;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Handlers.Nipkg.RemoteFeed;

namespace Gcd.Handlers.Nipkg.FeedGit;

public record AddPackageResponse(string Result);

public class AddPackageHandler(
    IMediator _mediator,
    IFileSystem _fs,
    IRemoteFileSystem _rfs)
    : IRequestHandler<AddPackageToRemoteFeedRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(AddPackageToRemoteFeedRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (feedDefinition, packagePath, cmdPath, useAbs, createFeed) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = localFeedDef
            .Map((arg) => PackageFilePath.Of(arg.Feed, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => PullFeed(feedDefinition, localFeedDef.Value.Feed))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value, packagePath, cmdPath, useAbs, createFeed: true))
            .Bind(() => PushFeed(feedDefinition, localFeedDef.Value.Feed));
        return Result.Success();
    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));

    private async Task<Result> PullFeed(FeedDefinitionGit feedDefinition, LocalDirPath checkoutPath) =>
        Result.Try(() => CloneRepositoryWithCredentials(feedDefinition, checkoutPath), ex => ex.Message).MapError(errr => $"PULL:{errr}");


    public void CloneRepositoryWithCredentials(FeedDefinitionGit feedDefinition, LocalDirPath checkoutPath)
    {
        var (address, branch, username, password, _, _) = feedDefinition;
        var fetchOptions = new FetchOptions
        {
            CredentialsProvider = (url, user, cred) =>
                new UsernamePasswordCredentials
                {
                    Username = username.Value,
                    Password = password.Value
                }
        };

        var cloneOptions = new CloneOptions(fetchOptions)
        {
            BranchName = branch.Value
        };

        Repository.Clone(address.Value, checkoutPath.Value, cloneOptions);

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




