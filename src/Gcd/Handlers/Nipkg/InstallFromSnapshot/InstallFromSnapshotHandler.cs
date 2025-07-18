using CSharpFunctionalExtensions;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromSnapshot;

public record InstallFromInstallerDirectoryResponse();
public record InstallFromSnapshotRequest(
    Snapshot Snapshot, Maybe<string> PackageMatchPattern, bool SimulateInstallation
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
    
    
    private async Task<Result> InstallFeeds(Snapshot snapshot)
    {
        var results = new List<Result>();
        foreach (var feed  in snapshot.Feeds)
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
    
    private async Task<Result> RemoveFeeds(Snapshot snapshot)
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
    
    private async Task<Result> InstallPackages(Snapshot snapshot, bool simulateInstallation)
    {

        var packageToInstalls = snapshot.Packages.Select(x =>
            new PackageToInstall(x.Package, x.Version)).ToList();

        var request = new InstallRequest(packageToInstalls,true,
            true, simulateInstallation, true, false, true);
        var result = await _nipkgService.InstallAsync(request);
        return result;
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> InstallFromSnapshotAsync(
        this IMediator mediator,
        Snapshot  snapshot,
        Maybe<string> packageMatchPattern,
        bool simulateInstallation = false,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new InstallFromSnapshotRequest(snapshot, packageMatchPattern,simulateInstallation), cancellationToken);
}