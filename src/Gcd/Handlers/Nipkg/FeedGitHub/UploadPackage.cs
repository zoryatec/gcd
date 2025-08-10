using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGitHub;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemGit rfs)
    : IRequestHandler<UploadPackageRequest<FeedDefinitionGitHub>, Result>
{
    public async Task<Result> Handle(UploadPackageRequest<FeedDefinitionGitHub> request, CancellationToken cancellationToken)
    {
        var (feedDef, packageFilePaths) = request;
        var owner = GitHubReleaseService.GetOwnerFromRepoUrl(feedDef.Address.Value);
        var repo = GitHubReleaseService.GetRepoNameFromRepoUrl(feedDef.Address.Value);
        var service = new GitHubReleaseService(feedDef.Password.Value, owner, repo);
        foreach (var packageFilePath in packageFilePaths)
        {
            var latestCommitOnBranch = await service.GetLatestCommitShaAsync(feedDef.BrancName.Value);
            await service.CreateTagAsync("0.1.0", latestCommitOnBranch, "Initial release");
            var release = await service.CreateReleaseAsync("0.1.0", "Initial Release", "This is the first release.", false);
            await service.UploadAssetAsync(release, packageFilePath.Value, "application/octet-stream");
        }
        return  Result.Success();
    }
}
