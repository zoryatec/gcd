using CSharpFunctionalExtensions;

namespace Gcd.NiPackageManager.Abstractions;

public interface ISnapshotSerializer
{
    public Task<Result<string>> SerializeAsync(Snapshot snapshot, string path);
    public Task<Result<Snapshot>> DeserializeAsync(string snapshotJson);
}