namespace Gcd.Snapshot;

public record PackageDefinitionDto(string Package, string Version, string Description, string Depends,
    string StoreProduct, string UserVisible, string Section);
public record FeedDefinitionDto(string Name, string Uri);
public record SnapshotDto(IReadOnlyList<PackageDefinitionDto> Packages,IReadOnlyList<FeedDefinitionDto> Feeds);