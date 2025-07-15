using CSharpFunctionalExtensions;

namespace Gcd.NiPackageManager.Abstractions;


public record FeedDefinition(string Name, string Uri);
public record AddFeedRequest(FeedDefinition FeedToAdd);
public record AddFeedResponse(FeedDefinition AddedFeed);
public record RemoveFeedsRequest(FeedDefinition FeedToRemove);
public record RemoveFeedsResponse(FeedDefinition RemovedFeed);

public record PackageDefinition(string Package, string Version, string Description, string Depends);

public record InfoInstalledRequest(string Pattern);
public record InfoInstalledResponse(IReadOnlyList<PackageDefinition> Packages);

public record PackageToInstall(string Package, string Version);

public enum ExitCode
{
    
}

public record InstallRequest( IReadOnlyList<PackageToInstall> PackagesToInstalls, bool AcceptEulas = false, 
    bool AssumeYes = false, bool Simulate = false, bool ForceLocked = false, bool SuppressIncompatibilityErrros = false,
    bool Verbose = false );

public interface INiPackageManagerService
{
    public Task<Result> Install(InstallRequest request);
    
    public Task<Result<InfoInstalledResponse>> InfoInstalledAsync(InfoInstalledRequest request);
    
    public Task<Result<AddFeedResponse>> AddFeedAsync(AddFeedRequest request);
    
    public Task<Result<RemoveFeedsResponse>> RemoveFeedAsync(RemoveFeedsRequest request);
    public Task<Result> VersionAsync();
    
}