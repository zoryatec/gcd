using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services.RemoteFileSystem;
using LibGit2Sharp;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGit;


public class PullFeedMetaHandler(IFileSystem _fs, IRemoteFileSystemGit _rfs)
    : IRequestHandler<PullFeedMetaRequest<FeedDefinitionGit>, Result>
{
    public async Task<Result> Handle(PullFeedMetaRequest<FeedDefinitionGit> request, CancellationToken cancellationToken)
    {
        var (remoteFeedDef, localFeedDef) = request;

        var checkoutDir =  await _fs.GenerateTempDirectoryAsync();
        var checkoutFeed = FeedDefinitionLocal.Of(checkoutDir.Value);

        var pullResult = await checkoutFeed
            .Bind((x) => _rfs.Clone(remoteFeedDef.Address, remoteFeedDef.BrancName, remoteFeedDef.UserName, remoteFeedDef.Password, checkoutFeed.Value.Feed));


         return await pullResult
            .Bind(() => _fs.CreateDirAsync(localFeedDef.Feed)) // this actually create if not exist
            .Bind(() => _fs.CopyFileAsync(checkoutFeed.Value.Package, localFeedDef.Package, overwrite: true))
            .Bind(() => _fs.CopyFileAsync(checkoutFeed.Value.PackageGz, localFeedDef.PackageGz, overwrite: true))
            .Bind(() => _fs.CopyFileAsync(checkoutFeed.Value.PackageStamps, localFeedDef.PackageStamps, overwrite: true));
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