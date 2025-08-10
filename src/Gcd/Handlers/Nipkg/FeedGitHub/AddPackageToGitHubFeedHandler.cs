using CSharpFunctionalExtensions;
using MediatR;
using Gcd.Model.Config;
using Gcd.Services.RemoteFileSystem;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Model.Nipkg.FeedDefinition;
using Gcd.Model.Nipkg.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;

namespace Gcd.Handlers.Nipkg.Shared;

public record AddPackageToGitHubFeedRequest(
    FeedDefinitionGitHub AzFeedDef,
    IReadOnlyList<IPackageFileDescriptor> PackagePaths,
    NipkgCmdPath CmdPath,
    UseAbsolutePath useAbsolutePath,
    bool createFeed = false
)
    : IRequest<Result>;

public class AddPackageToGitHubFeedHandler(
    IMediator _mediator,
    IFileSystem _fs,
    IRemoteFileSystemAzBlob _rfs,
    IWebDownload _webDownload)
    : IRequestHandler<AddPackageToGitHubFeedRequest,Result>

{
    public async Task<Result> Handle(AddPackageToGitHubFeedRequest request, CancellationToken cancellationToken)
    {
        var (azFeedDef, packagePaths, cmdPath, useAbs, createFeed) = request;

        var localFeedDef = await CreateTempFeedDefinition();

        var insideFeedPkgPaths = new List<PackageLocalFilePath>();
        var uploadedPackagePaths = new List<PackageHttpPath>();
        var tempDir = await _fs.CreateTempDirPathAsync();
        await _fs.CreateDirAsync(tempDir.Value);
        foreach (var packagePath in packagePaths)
        {
            var repoName = GetRepoNameFromRepoUrl(azFeedDef.Address.Value);
            var ownerName = GetOwnerFromRepoUrl(azFeedDef.Address.Value);
            var version = GetVersionFromPackagePath(packagePath.FileName.Value);
            
            var url = GetGitHubReleaseAssetUrl(ownerName, repoName, version, packagePath.FileName.Value);

            var tempFilePath = new PackageLocalFilePath(tempDir.Value, packagePath.FileName);
            await DownloadFile(packagePath, tempFilePath);
            
            var insideFeedPkgPath = localFeedDef
                .Map((arg) => PackageLocalFilePath.Of(arg.Feed, tempFilePath.FileName));
            if (insideFeedPkgPath.IsFailure) return insideFeedPkgPath;
            insideFeedPkgPaths.Add(tempFilePath);
            
            var uploadedPackagePath = PackageHttpPath.Of(url);
            if (uploadedPackagePath.IsFailure) return uploadedPackagePath;
            uploadedPackagePaths.Add(uploadedPackagePath.Value);
            
        }

        return await Result.Combine(localFeedDef)
            .Bind(() => _mediator.PullFeedMetaAsync(azFeedDef, localFeedDef.Value, cancellationToken))
            .Bind(() => _mediator.UploadPackageAsync(azFeedDef, insideFeedPkgPaths, cancellationToken))
            .Bind(() => _mediator.AddToLocalFeedAsync(localFeedDef.Value, uploadedPackagePaths, cmdPath, useAbs, createFeed,
                cancellationToken))
            .Bind(() => _mediator.PushFeedMetaDataAsync(azFeedDef, localFeedDef.Value, cancellationToken));

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
    public static string GetOwnerFromRepoUrl(string repoUrl)
        {
            if (string.IsNullOrWhiteSpace(repoUrl))
                throw new ArgumentException("Repository URL cannot be null or empty.", nameof(repoUrl));

            // Remove protocol
            var url = repoUrl.Replace("https://", "").Replace("http://", "").Replace("git@", "");
            // Remove possible .git suffix
            if (url.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
                url = url.Substring(0, url.Length - 4);

            // For git@github.com:owner/repo or github.com/owner/repo
            var parts = url.Split(new[] { '/', ':' }, StringSplitOptions.RemoveEmptyEntries);
            // Find 'github.com' and get next two parts
            for (int i = 0; i < parts.Length - 2; i++)
            {
                if (parts[i].EndsWith("github.com", StringComparison.OrdinalIgnoreCase))
                {
                    return parts[i + 1];
                }
            }
            // Fallback: try second part
            if (parts.Length >= 2)
                return parts[parts.Length - 2];

            throw new ArgumentException($"Could not extract owner from repo URL: {repoUrl}");
        }

        public static string GetRepoNameFromRepoUrl(string repoUrl)
        {
            if (string.IsNullOrWhiteSpace(repoUrl))
                throw new ArgumentException("Repository URL cannot be null or empty.", nameof(repoUrl));

            // Remove protocol
            var url = repoUrl.Replace("https://", "").Replace("http://", "").Replace("git@", "");
            // Remove possible .git suffix
            if (url.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
                url = url.Substring(0, url.Length - 4);

            // For git@github.com:owner/repo or github.com/owner/repo
            var parts = url.Split(new[] { '/', ':' }, StringSplitOptions.RemoveEmptyEntries);
            // Find 'github.com' and get next two parts
            for (int i = 0; i < parts.Length - 2; i++)
            {
                if (parts[i].EndsWith("github.com", StringComparison.OrdinalIgnoreCase))
                {
                    return parts[i + 2];
                }
            }
            // Fallback: try last part
            if (parts.Length >= 2)
                return parts[parts.Length - 1];

            throw new ArgumentException($"Could not extract repo name from repo URL: {repoUrl}");
        }
    
    public static string GetGitHubReleaseAssetUrl(string owner, string repo, string tag, string assetFileName)
    {
        // Standard GitHub release asset URL format
        return $"https://github.com/{owner}/{repo}/releases/download/{tag}/{assetFileName}";
    }

    private async Task<Result<FeedDefinitionLocal>> CreateTempFeedDefinition() =>
        await _fs.CreateTempDirPathAsync()
            .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));
    
    

}

