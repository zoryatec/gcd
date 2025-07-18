using CSharpFunctionalExtensions;
using Gcd.NiPackageManager.Abstractions;

namespace Gcd.NiPackageManager;

public static class SnapshotExtensions
{
    public static Snapshot WherePackagesMatchPattern(
        this Snapshot snapshot, string pattern)
    {
        var packages = snapshot.Packages
            .Where(p => p.Package.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        snapshot = snapshot with { Packages = packages };
        return snapshot;
    }
    
    public static Result<Snapshot> FilterPackages(
        this Snapshot snapshot, Maybe<string> packageMatchPattern, bool selectStoreProducts = false)
    {
        List<PackageDefinition> selectedPackages = [];
        List<PackageDefinition> matchedPackages = [];
        if (packageMatchPattern.HasValue)
        {
            matchedPackages = snapshot.Packages
                .Where(p => p.Package.Contains(packageMatchPattern.Value, StringComparison.OrdinalIgnoreCase))
                .ToList();
            selectedPackages.AddRange(matchedPackages);
        }
    
        List<PackageDefinition> storeProductsPackages = snapshot.Packages
            .Where(p => p.StoreProduct == "yes").ToList();
         
        selectedPackages.AddRange(storeProductsPackages);

        var distinct = selectedPackages.Distinct().ToList();

        snapshot = snapshot with { Packages = distinct};
        return Result.Success(snapshot);
    }
}