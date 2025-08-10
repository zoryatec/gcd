using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.Common;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Services;
using Gcd.Services.RemoteFileSystem;
using MediatR;

namespace Gcd.Handlers.Nipkg.FeedGitHub;


public class UploadPackage(IFileSystem _fs, IRemoteFileSystemGit rfs,IWebDownload _webDownload)
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

            var packageVersion = GetVersionFromPackagePath(packageFilePath.Value);
            var latestCommitOnBranch = await service.GetLatestCommitShaAsync(feedDef.BrancName.Value);
            await service.CreateTagAsync(packageVersion, latestCommitOnBranch, "Initial release");
            var release = await service.CreateReleaseAsync(packageVersion, packageVersion, "", false);
            await service.UploadAssetAsync(release, packageFilePath.Value, "application/octet-stream");
        }
        return  Result.Success();
    }
    
    public static string GetVersionFromPackagePath(string packagePath)
    {
        if (string.IsNullOrWhiteSpace(packagePath))
            throw new ArgumentException("Package path cannot be null or empty.", nameof(packagePath));
        var fileName = Path.GetFileNameWithoutExtension(packagePath);
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException($"Could not extract file name from path: {packagePath}");
        // Example: fcc61482-09df-4fa1-aab5-81fe03fca524_99.88.77.66_windows_x64
        var parts = fileName.Split('_');
        if (parts.Length < 2)
            throw new ArgumentException($"File name does not match expected pattern: {fileName}");
        // Version is the second part
        return parts[1];
    }
    
    private async Task<Result> DownloadFile(IFileDescriptor sourceDescriptor, ILocalFilePath destinationPath, bool overwrite = false)
    {
        return sourceDescriptor switch
        {
            ILocalFilePath source => await _fs.CopyFileAsync(source, destinationPath, overwrite: overwrite),
            IWebFileUri source => await _webDownload.DownloadFileAsync(source, destinationPath),
            _ => throw new InvalidOperationException(sourceDescriptor.GetType().Name)
        };
    }
}
