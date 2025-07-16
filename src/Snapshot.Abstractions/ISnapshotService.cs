using CSharpFunctionalExtensions;

namespace Snapshot.Abstractions;

public interface ISnapshotService
{
    public Task<Result<string>> InstallPackagesFromSnapshotAsync(Snapshot snapshot);
}