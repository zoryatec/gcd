using CSharpFunctionalExtensions;

namespace Snapshot.Abstractions;

public interface IInstallPackagesFromSnapshotService
{
    public Task<Result<string>> InstallPackagesFromSnapshotAsync(Snapshot snapshot);
}