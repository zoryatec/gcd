using CSharpFunctionalExtensions;
using Gcd.NiPackageManager.Abstractions;

namespace Gcd.NiPackageManager;

public class NiPackageManagerExtendedService(INiPackageManagerService _nipkgService) : INiPackageManagerExtendedService
{
    public async Task<Result> InstallFeedAsync(IReadOnlyList<FeedDefinition> feeds)
    {
        var results = new List<Result>();
        foreach (var feed  in feeds)
        {
            var request = new AddFeedRequest(feed);
            var result  = await _nipkgService.AddFeedAsync(request);
            results.Add(result);
        }

        var resultc = Result.Combine(results);
        
        if(resultc.IsFailure) { return resultc;}
        var resultUpdate = await _nipkgService.UpdateAsync();
        return resultUpdate;
    }
    
    public async Task<Result> InstallFeedAsync(FeedDefinition feed)
    {
        var results = new List<Result>();

        var request = new AddFeedRequest(feed);
        var result  = await _nipkgService.AddFeedAsync(request);
        results.Add(result);
        

        var resultc = Result.Combine(results);
        
        if(resultc.IsFailure) { return resultc;}
        var resultUpdate = await _nipkgService.UpdateAsync();
        return resultUpdate;
    }
    
    public async Task<Result> RemoveFeedAsync(IReadOnlyList<FeedDefinition> feeds)
    {
        var results = new List<Result>();
        foreach (var feed  in feeds)
        {
            var feedDefinition = new FeedDefinition(feed.Name, feed.Uri);
            var request = new RemoveFeedsRequest(feedDefinition);
            var result  = await _nipkgService.RemoveFeedAsync(request);
            results.Add(result);
        }
        return Result.Combine(results);
    }
    
    public async Task<Result> InstallPackageAsync(IReadOnlyList<PackageDefinition> packages, bool simulateInstallation)
    {
        var packageToInstalls = packages.Select(x =>
            new PackageToInstall(x.Package, x.Version)).ToList();

        var request = new InstallRequest(packageToInstalls,true,
            true, simulateInstallation, true, false, true);
        var result = await _nipkgService.InstallAsync(request);
        return result;
    }
    
    public async Task<Result> InstallPackageAsync(PackageDefinition package, bool simulateInstallation)
    {
        var packageToInstall = new PackageToInstall(package.Package, package.Version);

        var request = new InstallRequest(new List<PackageToInstall>{packageToInstall},true,
            true, simulateInstallation, true, false, true);
        var result = await _nipkgService.InstallAsync(request);
        return result;
    }
}