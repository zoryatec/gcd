using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using MediatR;

namespace Gcd.Handlers.Nipkg.InstallFromSnapshot;

public record InstallFromSnapshotFileRequest(
    LocalFilePath SnapshotFilePath
    ) : IRequest<Result>;


public class InstallFromSnapshotFileHandler(IMediator _mediator)
    : IRequestHandler<InstallFromSnapshotFileRequest, Result>
{
    public async Task<Result> Handle(InstallFromSnapshotFileRequest request, CancellationToken cancellationToken)
    {
        var snapshotFilePath = request.SnapshotFilePath;
        var result =  await new SnapshotSerializerJson().
                DeserializeAsync(snapshotFilePath.Value)
                .Bind(snapshot=> _mediator.InstallFromSnapshotAsync(snapshot,Maybe.None,
                    true,cancellationToken));

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