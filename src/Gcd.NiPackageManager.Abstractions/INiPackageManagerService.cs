using CSharpFunctionalExtensions;

namespace Gcd.NiPackageManager.Abstractions;

public record PackageDefinition(string Package, string Version, string Description = "", string Depends = "",
    string StoreProduct = "", string UserVisible = "", string Section = "");
public record FeedDefinition(string Name, string Uri);
public record AddFeedRequest(FeedDefinition FeedToAdd);
public record AddFeedResponse(FeedDefinition AddedFeed);
public record RemoveFeedsRequest(FeedDefinition FeedToRemove);
public record RemoveFeedsResponse(FeedDefinition RemovedFeed);

public record InfoInstalledRequest(string Pattern);
public record InfoInstalledResponse(IReadOnlyList<PackageDefinition> Packages);

public record PackageToInstall(string Package, string Version);

public enum ExitCode
{
 FailedToUpateOneOrMoreFeeds = -125951 ,
 PluginrReturnedOneOrMoreErrorsAtTheBeginningOfTransaction =-125090
}

public record InstallRequest( IReadOnlyList<PackageToInstall> PackagesToInstalls, bool AcceptEulas = false, 
    bool AssumeYes = false, bool Simulate = false, bool ForceLocked = true, bool SuppressIncompatibilityErrros = false,
    bool Verbose = true, bool AllowDowngrade = true, bool AllowUninstall = true, bool InstallAlsoUpgrades = true,
    bool IncludeRecommended = false);

public record RemoveRequest( IReadOnlyList<PackageToInstall> PackagesToRemove,
    bool AssumeYes = false, bool Simulate = false, bool ForceLocked = false, bool SuppressIncompatibilityErrros = false,
    bool Verbose = false );

public interface INiPackageManagerService
{
    public Task<Result> InstallAsync(InstallRequest request);
    public Task<Result> RemoveAsync(RemoveRequest request);
    
    public Task<Result<InfoInstalledResponse>> InfoInstalledAsync(InfoInstalledRequest request);
    
    public Task<Result<string>> AddFeedAsync(AddFeedRequest request);
    
    public Task<Result<string>> UpdateAsync();
    
    public Task<Result<string>> RemoveFeedAsync(RemoveFeedsRequest request);
    public Task<Result> VersionAsync();
    
}