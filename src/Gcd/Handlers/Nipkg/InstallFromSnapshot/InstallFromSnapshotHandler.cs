using CSharpFunctionalExtensions;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromSnapshot;

public record InstallFromInstallerDirectoryResponse();
public record InstallFromSnapshotRequest(
    Snapshot Snapshot, Maybe<string> PackageMatchPattern, bool SimulateInstallation
) : IRequest<Result>;


public class InstallFromSnapshotHandler(IMediator _mediator, INiPackageManagerExtendedService _nipkgService)
    : IRequestHandler<InstallFromSnapshotRequest, Result>
{
    public async Task<Result> Handle(InstallFromSnapshotRequest request, CancellationToken cancellationToken)
    {
        var snapshot = request.Snapshot;
        return  await _nipkgService.InstallFeedAsync(snapshot.Feeds).Map(() => snapshot)
            .Bind((snap) => snap.FilterPackages(request.PackageMatchPattern, true))
            .Bind((snap) => snap.RemoveDuplicatePackagesByHighestVersion())
            .Bind(snap => _nipkgService.InstallPackageAsync(snap.Packages, request.SimulateInstallation).Map(() => snap))
            .Finally(snap => _nipkgService.RemoveFeedAsync(snapshot.Feeds));
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