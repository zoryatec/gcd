using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using Gcd.Snapshot;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromSnapshot;

public record InstallFromSnapshotRequest(
    LocalFilePath SnapshotFilePath
    ) : IRequest<Result>;


public class InstallFromSnapshotHandler(IMediator _mediator, INiPackageManagerService _nipkgService, 
    SnapshotService snapshotService)
    : IRequestHandler<InstallFromSnapshotRequest, Result>
{
    public async Task<Result> Handle(InstallFromSnapshotRequest request, CancellationToken cancellationToken)
    {

        var snapshotSerializer = new SnapshotSerializerJson();
        var snapshotFilePath = request.SnapshotFilePath;

        var result =  await snapshotSerializer.DeserializeAsync(snapshotFilePath.Value)
            .Bind(snapshotService.InstallPackagesFromSnapshotAsync);

        return result;
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> InstallFromSnapshotRequest(
        this IMediator mediator,
        LocalFilePath snapshotFilePath,
        CancellationToken cancellationToken = default
        )
        => await mediator.Send(new InstallFromSnapshotRequest(snapshotFilePath), cancellationToken);
}