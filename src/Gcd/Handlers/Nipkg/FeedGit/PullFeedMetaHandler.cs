using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;
using Gcd.Services.FileSystem;
using Gcd.Services.RemoteFileSystem;
using LibGit2Sharp;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGit;


public class PullFeedMetaHandler(IFileSystem _fs, RemoteFileSystemGit _rfs)
    : IRequestHandler<PullFeedMetaRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;
        return await PullFeed(remoteFeedDef, localFeedDef.Feed);
    }

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
}