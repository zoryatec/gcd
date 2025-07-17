using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Snapshot;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using Gcd.Snapshot;
using MediatR;
using PackageDefinition = Snapshot.Abstractions.PackageDefinition;

namespace Gcd.Handlers.Nipkg.InstallFromSnapshot;

public record InstallFromInstallerDirectoryResponse();
public record InstallFromSnapshotRequest(
    global::Snapshot.Abstractions.Snapshot Snapshot, Maybe<string> PackageMatchPattern, bool SimulateInstallation
) : IRequest<Result>;


public class InstallFromSnapshotHandler(IMediator _mediator, INiPackageManagerService _nipkgService)
    : IRequestHandler<InstallFromSnapshotRequest, Result>
{
    public async Task<Result> Handle(InstallFromSnapshotRequest request, CancellationToken cancellationToken)
    {

        var snapshot = request.Snapshot;
        var resultFeed = await InstallFeeds(snapshot);
        if (resultFeed.IsFailure) { return Result.Failure(resultFeed.Error); }
        
        snapshot = snapshot.FilterPackages(request.PackageMatchPattern, true);
        var result = await InstallPackages(snapshot,request.SimulateInstallation); 

        var resultRemove = await RemoveFeeds(snapshot);

        
        return Result.Combine(resultRemove,result);
    }
    
    
    private async Task<Result> InstallFeeds(global::Snapshot.Abstractions.Snapshot snapshot)
    {
        var results = new List<Result>();
        foreach (var feed  in snapshot.Feeds)
        {
            var feedDefinition = new FeedDefinition(feed.Name, feed.Uri);
            var request = new AddFeedRequest(feedDefinition);
            var result  = await _nipkgService.AddFeedAsync(request);
            results.Add(result);
        }

        var resultc = Result.Combine(results);
        
        if(resultc.IsFailure) { return resultc;}
        var resultUpdate = await _nipkgService.UpdateAsync();
        return resultUpdate;
    }
    
    private async Task<Result> RemoveFeeds(global::Snapshot.Abstractions.Snapshot snapshot)
    {
        var results = new List<Result>();
        foreach (var feed  in snapshot.Feeds)
        {
            var feedDefinition = new FeedDefinition(feed.Name, feed.Uri);
            var request = new RemoveFeedsRequest(feedDefinition);
            var result  = await _nipkgService.RemoveFeedAsync(request);
            results.Add(result);
        }
        return Result.Combine(results);
    }
    
    private async Task<Result> InstallPackages(global::Snapshot.Abstractions.Snapshot snapshot, bool simulateInstallation)
    {

        var packageToInstalls = snapshot.Packages.Select(x =>
            new PackageToInstall(x.Package, x.Version)).ToList();

        var request = new InstallRequest(packageToInstalls,true,
            true, simulateInstallation, true, false, true);
        var result = await _nipkgService.InstallAsync(request);
        return result;
    }
}

public static class SnapshotExtensions
{
    public static global::Snapshot.Abstractions.Snapshot WherePackagesMatchPattern(
        this global::Snapshot.Abstractions.Snapshot snapshot, string pattern)
    {
        var packages = snapshot.Packages
            .Where(p => p.Package.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        snapshot = snapshot with { Packages = packages };
        return snapshot;
    }
    
    public static global::Snapshot.Abstractions.Snapshot FilterPackages(
        this global::Snapshot.Abstractions.Snapshot snapshot, Maybe<string> packageMatchPattern, bool selectStoreProducts = false)
    {
        List<global::Snapshot.Abstractions.PackageDefinition> selectedPackages = [];
        List<global::Snapshot.Abstractions.PackageDefinition> matchedPackages = [];
        if (packageMatchPattern.HasValue)
        {
            matchedPackages = snapshot.Packages
                .Where(p => p.Package.Contains(packageMatchPattern.Value, StringComparison.OrdinalIgnoreCase))
                .ToList();
            selectedPackages.AddRange(matchedPackages);
        }
    
         List<PackageDefinition> storeProductsPackages = snapshot.Packages
            .Where(p => p.StoreProduct == "yes").ToList();
         
         selectedPackages.AddRange(storeProductsPackages);

         var distinct = selectedPackages.Distinct().ToList();

         snapshot = snapshot with { Packages = distinct};
        return snapshot;
    }

}

public static class MediatorExtensions
{
    public static async Task<Result> InstallFromSnapshotAsync(
        this IMediator mediator,
        global::Snapshot.Abstractions.Snapshot  snapshot,
        Maybe<string> packageMatchPattern,
        bool simulateInstallation = false,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new InstallFromSnapshotRequest(snapshot, packageMatchPattern,simulateInstallation), cancellationToken);
}