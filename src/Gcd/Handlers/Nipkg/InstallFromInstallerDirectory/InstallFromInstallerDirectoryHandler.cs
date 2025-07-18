using CSharpFunctionalExtensions;
using Gcd.Handlers.Nipkg.InstallFromSnapshot;
using Gcd.Handlers.Nipkg.Snapshot;
using Gcd.LocalFileSystem.Abstractions;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;

public record InstallFromInstallerDirectoryRequest(
    LocalDirPath InstallerDirectoryPath, Maybe<string> PackageMatchPattern, bool SimulateInstallation
) : IRequest<Result>;

public class InstallFromInstallerDirectoryHandler(IMediator mediator)
    : IRequestHandler<InstallFromInstallerDirectoryRequest, Result>
{
    public async Task<Result> Handle(InstallFromInstallerDirectoryRequest request, CancellationToken cancellationToken)
    {
      return await mediator.Send(new CreateSnapshotFromInstallerRequest(request.InstallerDirectoryPath), cancellationToken)
            .Bind(response => mediator.InstallFromSnapshotAsync(response.Snapshot,
                request.PackageMatchPattern,
                request.SimulateInstallation,
                cancellationToken));
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