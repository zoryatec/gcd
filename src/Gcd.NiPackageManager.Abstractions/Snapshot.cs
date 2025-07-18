namespace Gcd.NiPackageManager.Abstractions;


public record Snapshot(IReadOnlyList<PackageDefinition> Packages,IReadOnlyList<FeedDefinition> Feeds);