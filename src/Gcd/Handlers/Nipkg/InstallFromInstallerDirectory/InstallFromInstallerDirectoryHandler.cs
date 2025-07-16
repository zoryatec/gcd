



using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.InstallFromSnapshot;
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
        if (snapshotResult.IsFailure)
        {
            return Result.Failure(snapshotResult.Error);
        }
        var snapshot = snapshotResult.Value.Snapshot;
        
        return await _mediator.InstallFromSnapshotAsync(snapshot,
            request.PackageMatchPattern,
            request.SimulateInstallation,
            cancellationToken);
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> InstallFromInstallerDirectoryAsync(
        this IMediator mediator,
        LocalDirPath installerDirectoryPath,
        Maybe<string> packageMatchPattern,
        bool simulateInstallation = false,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new InstallFromInstallerDirectoryRequest(installerDirectoryPath, packageMatchPattern,simulateInstallation), cancellationToken);
}