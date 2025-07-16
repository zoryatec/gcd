using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager.Abstractions;
using Gcd.Snapshot;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromSnapshot;

public record InstallFromSnapshotFileRequest(
    LocalFilePath SnapshotFilePath
    ) : IRequest<Result>;


public class InstallFromSnapshotFileHandler(IMediator _mediator, INiPackageManagerService _nipkgService, 
    SnapshotService snapshotService)
    : IRequestHandler<InstallFromSnapshotFileRequest, Result>
{
    public async Task<Result> Handle(InstallFromSnapshotFileRequest fileRequest, CancellationToken cancellationToken)
    {

        var snapshotSerializer = new SnapshotSerializerJson();
        var snapshotFilePath = fileRequest.SnapshotFilePath;

        var result =  await snapshotSerializer.DeserializeAsync(snapshotFilePath.Value)
            .Bind(snapshotService.InstallPackagesFromSnapshotAsync);

        return result;
    }
}

public static class MediatorFileExtensions
{
    public static async Task<Result> InstallFromSnapshotRequest(
        this IMediator mediator,
        LocalFilePath snapshotFilePath,
        CancellationToken cancellationToken = default
        )
        => await mediator.Send(new InstallFromSnapshotFileRequest(snapshotFilePath), cancellationToken);
}