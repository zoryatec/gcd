using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.NiPackageManager;
using Gcd.NiPackageManager.Abstractions;
using MediatR;
using Providers.Abstractions;

namespace Gcd.Handlers.Nipkg.SnapshotManagment;


public record CreateSnapshotFromInstallerResponse(Snapshot Snapshot);
public record CreateSnapshotFromInstallerRequest(
    LocalDirPath InstallerDirectory
) : IRequest<Result<CreateSnapshotFromInstallerResponse>>;


public class CreateFromInstallerDirectoryHandler(IMediator _mediator, IInstallerDirectoryProvider  installerDirectoryProvider)
    : IRequestHandler<CreateSnapshotFromInstallerRequest, Result<CreateSnapshotFromInstallerResponse>>
{
    public async Task<Result<CreateSnapshotFromInstallerResponse>> Handle(CreateSnapshotFromInstallerRequest request, CancellationToken cancellationToken)
    {
        var installerDirectoryPath = request.InstallerDirectory;
        var installerDir = new InstallerDirectory(installerDirectoryPath);
        
        var snapshot =
            from directories in installerDirectoryProvider.GetMainFeedsDirectories(installerDir)
            from feedDefinitions in installerDirectoryProvider.GetFeedDefinitions(directories)
            from packageDefinitions in installerDirectoryProvider.GetPackageDefinitions(directories)
            select new Snapshot(packageDefinitions, feedDefinitions);
        
        return snapshot.Map(x => new CreateSnapshotFromInstallerResponse(x));
    }
}

public static class MediatorExtensions
{
    public static async Task<Result<CreateSnapshotFromInstallerResponse>> CreateSnapshotFromInstallerAsync(
        this IMediator mediator,
        LocalDirPath snapshotFilePath,
        CancellationToken cancellationToken = default
    )
        => await mediator.Send(new CreateSnapshotFromInstallerRequest(snapshotFilePath), cancellationToken);
}


// public record InstallerDirectory(LocalDirPath InstallerDirectoryPath)
// {
//
//     public LocalDirPath MainFeedsDirectoryPath => LocalDirPath.Of($"{InstallerDirectoryPath.Value}\\feeds").Value;
//     public LocalDirPath NiPackageManagerFeedsDirectoryPath = LocalDirPath.Of($"{InstallerDirectoryPath.Value}\\bin\\ni-package-manager-packages").Value;
// }