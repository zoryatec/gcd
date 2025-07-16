



using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Snapshot;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using Gcd.Snapshot;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;

public record InstallFromInstallerDirectoryResponse();
public record InstallFromInstallerDirectoryRequest(
    LocalDirPath InstallerDirectoryPath, Maybe<string> PackageMatchPattern, bool SimulateInstallation
) : IRequest<Result>;


public class InstallFromInstallerDirectoryHandler(IMediator _mediator, INiPackageManagerService _nipkgService)
    : IRequestHandler<InstallFromInstallerDirectoryRequest, Result>
{
    public async Task<Result> Handle(InstallFromInstallerDirectoryRequest request, CancellationToken cancellationToken)
    {
        var snapshotResult = await _mediator.Send(new CreateSnapshotFromInstallerRequest(request.InstallerDirectoryPath), cancellationToken);
        var snapshot = snapshotResult.Value.Snapshot;
        var resultFeed = await InstallFeeds(snapshot);
        if (resultFeed.IsFailure) { return Result.Failure(resultFeed.Error); }
        
        if (request.PackageMatchPattern.HasValue)
        {
            snapshot = snapshot.WherePackagesMatchPattern(request.PackageMatchPattern.Value);
        }
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

}

public static class MediatorExtensions
{
    public static async Task<Result> InstallFromInstallerDirectoryRequest(
        this IMediator mediator,
        LocalDirPath installerDirectoryPath,
        Maybe<string> packageMatchPattern,
        bool simulateInstallation = false,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new InstallFromInstallerDirectoryRequest(installerDirectoryPath, packageMatchPattern,simulateInstallation), cancellationToken);
}