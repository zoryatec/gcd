using CSharpFunctionalExtensions;

namespace Gcd.NiPackageManager.Abstractions;

public interface INiPackageManagerExtendedService
{
     Task<Result> InstallFeedAsync(IReadOnlyList<FeedDefinition> feeds);

    Task<Result> RemoveFeedAsync(IReadOnlyList<FeedDefinition> feeds);

    Task<Result> InstallPackageAsync(IReadOnlyList<PackageDefinition> packages, bool simulateInstallation);
    
}