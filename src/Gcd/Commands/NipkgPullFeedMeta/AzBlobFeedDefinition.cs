using CSharpFunctionalExtensions;
using Gcd.Services;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record AzBlobFeedDefinition
{
    public AzBlobUri Package { get; }
    public AzBlobUri PackageGz { get; }
    public AzBlobUri PackageStamps { get; }

    public static Result<AzBlobFeedDefinition> Of(AzBlobFeedUri feedUri)
    {
        var package = AzBlobUri.Create($"{feedUri.BaseUri}/Packages{feedUri.Query}");
        var packageGz = AzBlobUri.Create($"{feedUri.BaseUri}/Packages.gz{feedUri.Query}");
        var packageStamps = AzBlobUri.Create($"{feedUri.BaseUri}/Packages.stamps{feedUri.Query}");
        return Result
            .Combine(package, packageGz, packageStamps)
            .Map(() => new AzBlobFeedDefinition(package.Value, packageGz.Value, packageStamps.Value));
    }
    private AzBlobFeedDefinition(AzBlobUri package, AzBlobUri packageGz, AzBlobUri packageStamps)
    {
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
    }
}

