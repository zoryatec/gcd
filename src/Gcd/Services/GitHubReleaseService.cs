using System;
using System.IO;
using System.Threading.Tasks;
using Octokit;

namespace Gcd.Services
{
    public class GitHubReleaseService
    {
        private readonly GitHubClient _client;
        private readonly string _owner;
        private readonly string _repo;

        public GitHubReleaseService(string token, string owner, string repo)
        {
            _client = new GitHubClient(new ProductHeaderValue("GcdReleaseUploader"))
            {
                Credentials = new Credentials(token)
            };
            _owner = owner;
            _repo = repo;
        }

        public async Task<Release> CreateReleaseAsync(string tagName, string releaseName, string body, bool prerelease = false)
        {
            var newRelease = new NewRelease(tagName)
            {
                Name = releaseName,
                Body = body,
                Prerelease = prerelease
            };
            return await _client.Repository.Release.Create(_owner, _repo, newRelease);
        }

        public async Task<ReleaseAsset> UploadAssetAsync(Release release, string filePath, string contentType)
        {
            using (var stream = File.OpenRead(filePath))
            {
                var assetUpload = new ReleaseAssetUpload
                {
                    FileName = Path.GetFileName(filePath),
                    ContentType = contentType,
                    RawData = stream
                };
                return await _client.Repository.Release.UploadAsset(release, assetUpload);
            }
        }

        public async Task CreateTagAsync(string tagName, string commitSha, string message = null)
        {
            // Create annotated tag object
            var tag = new NewTag
            {
                Tag = tagName,
                Message = message ?? $"Tag {tagName}",
                Object = commitSha,
                Type = TaggedType.Commit,
            };
            var createdTag = await _client.Git.Tag.Create(_owner, _repo, tag);
            // Create reference (refs/tags/{tagName})
            await _client.Git.Reference.Create(_owner, _repo, new NewReference($"refs/tags/{tagName}", createdTag.Sha));
        }

        public async Task<string> GetLatestCommitShaAsync(string branchName)
        {
            // Get the reference for the branch
            var reference = await _client.Git.Reference.Get(_owner, _repo, $"heads/{branchName}");
            return reference.Object.Sha;
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

        public static string GenerateTagName(string baseName = "v")
        {
            // Use current UTC date and time for uniqueness, e.g. v20250810-153000
            var now = DateTime.UtcNow;
            return $"{baseName}{now:yyyyMMdd-HHmmss}";
        }
    }
}
