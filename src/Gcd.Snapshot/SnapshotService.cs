using CSharpFunctionalExtensions;
using Gcd.NiPackageManager.Abstractions;
using Snapshot.Abstractions;
using PackageDefinition = Gcd.NiPackageManager.Abstractions.PackageDefinition;

namespace Gcd.Snapshot;

public class SnapshotService(INiPackageManagerService _niPackageManagerService) : ISnapshotService
{
    public Task<Result<string>> InstallPackagesFromSnapshotAsync(global::Snapshot.Abstractions.Snapshot snapshot)
    {
        // add feeds
        // add packages
        var packages = MapPackages(snapshot.Packages);
        throw new NotImplementedException();
    }
    
    private  PackageDefinition MapPackages(IReadOnlyList<global::Snapshot.Abstractions.PackageDefinition> packageDefinitions)
    {
        throw new NotImplementedException();
    }
}