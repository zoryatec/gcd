using CSharpFunctionalExtensions;
using Gcd.Services;
using Gcd.Model;
using MediatR;
using Gcd.Commands.Nipkg.Feed.PullMetaDataAz;
using Gcd.Commands.Nipkg.Feed.PushMetaDataAz;
using Gcd.Model.Config;
using Gcd.Services.FileSystem;
using System.IO.Compression;
using Gcd.Model.File;
using Gcd.Commands.Nipkg.FeedLocal.AddPackageLocal;
using Gcd.Model.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using LibGit2Sharp;
using System.Reflection.Metadata;

namespace Gcd.Commands.Nipkg.FeedGit;

public static class MediatorExtensions
{
    public static async Task<Result> AddToGitlFeedAsync(this IMediator mediator,
        FeedDefinitionGit feedDefinition,
        IPackageFileDescriptor packageFileDescriptor,
        NipkgCmdPath CmdPath,
        UseAbsolutePath useAbsolutePath,
        bool createFeed = false,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new AddPackageRequest(feedDefinition, packageFileDescriptor, createFeed, useAbsolutePath, CmdPath), cancellationToken);
}

public record AddPackageRequest(FeedDefinitionGit FeedDefinition, IPackageFileDescriptor PackagePath,bool createFeed, UseAbsolutePath useAbsolutePath, NipkgCmdPath CmdPath) : IRequest<Result>;
public record AddPackageResponse(string Result);

public class AddPackageHandler(
    IMediator _mediator,
    IFileSystem _fs,
    IRemoteFileSystem _rfs)
    : IRequestHandler<AddPackageRequest, Result>
{
    public async Task<Result> Handle(AddPackageRequest request, CancellationToken cancellationToken)
    {
        var (feedDefinition, packagePath, createFeed, useAbs, cmdPath) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPath = localFeedDef
            .Map((arg) => PackageFilePath.Of(arg.Feed, packagePath.FileName));

        return await Result.Combine(localFeedDef, insideFeedPkgPath)
            .Bind(() => PullFeed(feedDefinition,localFeedDef.Value.Feed))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value, packagePath, cmdPath, useAbs, createFeed: true))
            .Bind(() => PushFeed(feedDefinition,localFeedDef.Value.Feed));

    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));

    private async Task<Result> PullFeed(FeedDefinitionGit feedDefinition,LocalDirPath checkoutPath)
    {
        CloneRepositoryWithCredentials(
            feedDefinition.Address.Value,
            checkoutPath.Value,
            feedDefinition.UserName.Value,
            feedDefinition.Password.Value);
        return Result.Success();
    }

    public void CloneRepositoryWithCredentials(string cloneUrl, string localPath, string username, string password)
    {
        var fetchOptions = new FetchOptions
        {
            CredentialsProvider = (url, user, cred) =>
                new UsernamePasswordCredentials
                {
                    Username = username,
                    Password = password
                }
        };

        var cloneOptions = new CloneOptions(fetchOptions)
        {
            BranchName = "initiaDev"
        };


        Repository.Clone(cloneUrl, localPath, cloneOptions);

        
    }

    private async Task<Result> PushFeed(FeedDefinitionGit feedDefinition,LocalDirPath checkoutPath)
    {
        CommitAndPush(
            checkoutPath.Value,
            "test",
            feedDefinition.UserName.Value,
            feedDefinition.Password.Value);
        return Result.Success();
    }


    public void CommitAndPush(string repoPath, string commitMessage, string username, string password)
    {
        using (var repo = new Repository(repoPath))
        {
            LibGit2Sharp.Commands.Stage(repo, "*");

            // Check if there are staged changes
            var status = repo.RetrieveStatus();
            if (status.IsDirty)
            {
                // Create a signature for the commit
                var author = new Signature("Your Name", "your-email@example.com", DateTime.Now);
                var committer = author;

                // Commit the changes
                repo.Commit(commitMessage, author, committer);
                Console.WriteLine("Changes committed.");
            }
            else
            {
                Console.WriteLine("No changes to commit.");
            }

            // Push changes to the remote repository
            var pushOptions = new PushOptions
            {
                CredentialsProvider = (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials
                    {
                        Username = username,
                        Password = password
                    }
            };
            Remote remote = repo.Network.Remotes["origin"];
            // Push the current branch
            repo.Network.Push(repo.Branches["initiaDev"], pushOptions);
            Console.WriteLine("Changes pushed to remote repository.");
        }
    }

 }




