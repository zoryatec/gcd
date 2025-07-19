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
    
    public static Result<Snapshot> RemoveDuplicatePackagesByHighestVersion(this Snapshot snapshot)
    {
        var uniquePackages = snapshot.Packages
            .GroupBy(p => p.Package)
            .Select(group =>
            {
                if (group.Count() == 1)
                    return group.First();

                // Select the package with the highest version
                return group.Aggregate((max, current) =>
                    SnapshotExtensions.CompareVersions(current.Version, max.Version) > 0 ? current : max);
            })
            .ToList();

        return Result.Success(snapshot with { Packages = uniquePackages });
    }
    
    public static int CompareVersions(string version1, string version2)
    {
        if (version1 == version2)
        {
            // Versions are identical strings
            return 0;
        }

        try
        {
            var v1 = Version.Parse(version1);
            var v2 = Version.Parse(version2);
            // Successfully parsed as System.Version
            return v1.CompareTo(v2);
        }
        catch
        {
            // Failed to parse as System.Version

            // Strip build metadata (+xxx) and pre-release info (-xxx)
            string cleanVersion1 = System.Text.RegularExpressions.Regex.Replace(version1, @"\+.*$", "");
            cleanVersion1 = System.Text.RegularExpressions.Regex.Replace(cleanVersion1, @"-.*$", "");
            string cleanVersion2 = System.Text.RegularExpressions.Regex.Replace(version2, @"\+.*$", "");
            cleanVersion2 = System.Text.RegularExpressions.Regex.Replace(cleanVersion2, @"-.*$", "");

            try
            {
                var v1 = Version.Parse(cleanVersion1);
                var v2 = Version.Parse(cleanVersion2);
                int result = v1.CompareTo(v2);

                // If core versions are equal, compare the original strings
                if (result == 0 && version1 != version2)
                {
                    return string.Compare(version1, version2, StringComparison.Ordinal);
                }

                return result;
            }
            catch
            {
                // Fall back to string comparison
                return string.Compare(version1, version2, StringComparison.Ordinal);
            }
        }
    }
}