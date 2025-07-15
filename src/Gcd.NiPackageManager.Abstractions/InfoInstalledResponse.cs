namespace Gcd.NiPackageManager.Abstractions;


public record PackageDefinition(string Package, string Version, string Description, string Depends);


public record InfoInstalledResponse(IReadOnlyList<PackageDefinition> Packages);