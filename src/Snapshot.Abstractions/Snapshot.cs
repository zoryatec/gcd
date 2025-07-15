namespace Snapshot.Abstractions;

public record PackageDefinition(string Package, string Version, string Description, string Depends);
public record FeedDefinition(string Name, string Uri);
public record Snapshot(IReadOnlyList<PackageDefinition> Packages,IReadOnlyList<FeedDefinition> Feeds);