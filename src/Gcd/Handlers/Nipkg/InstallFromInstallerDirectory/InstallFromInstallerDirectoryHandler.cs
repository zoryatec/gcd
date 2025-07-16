



using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.Snapshot;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using Gcd.Snapshot;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;

public record InstallFromInstallerDirectoryResponse();
public record InstallFromInstallerDirectoryRequest(
    LocalDirPath InstallerDirectoryPath
) : IRequest<Result>;


public class InstallFromInstallerDirectoryHandler(IMediator _mediator, INiPackageManagerService _nipkgService, 
    SnapshotService snapshotService)
    : IRequestHandler<InstallFromInstallerDirectoryRequest, Result>
{
    public async Task<Result> Handle(InstallFromInstallerDirectoryRequest request, CancellationToken cancellationToken)
    {
        var snapshotResult = await _mediator.Send(new CreateSnapshotFromInstallerRequest(request.InstallerDirectoryPath), cancellationToken);
        var snapshot = snapshotResult.Value.Snapshot;
        await InstallFeeds(snapshot);
        await InstallPackages(snapshot);
        var snapshotSerializer = new SnapshotSerializerJson();
        var snapshotFilePath = request.InstallerDirectoryPath;
        

        return Result.Success();
    }
    
    
    private async Task<Result> InstallFeeds(global::Snapshot.Abstractions.Snapshot snapshot)
    {
        foreach (var feed  in snapshot.Feeds)
        {
            var feedDefinition = new FeedDefinition(feed.Name, feed.Uri);
            var request = new AddFeedRequest(feedDefinition);
            await _nipkgService.AddFeedAsync(request);
        }
        return Result.Success();
    }
    
    private async Task<Result> InstallPackages(global::Snapshot.Abstractions.Snapshot snapshot)
    {

        var packageToInstalls = snapshot.Packages.Select(x =>
            new PackageToInstall(x.Package, x.Version)).ToList();

        var request = new InstallRequest(packageToInstalls,true,
            true, true, true, false, true);
        await _nipkgService.Install(request);
        return Result.Success();
    }
    
    
}

public static class MediatorExtensions
{
    public static async Task<Result> InstallFromInstallerDirectoryRequest(
        this IMediator mediator,
        LocalDirPath installerDirectoryPath,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new InstallFromInstallerDirectoryRequest(installerDirectoryPath), cancellationToken);
}