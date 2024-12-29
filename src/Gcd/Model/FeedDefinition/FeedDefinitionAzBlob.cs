using CSharpFunctionalExtensions;
using Gcd.Services;

namespace Gcd.Model.FeedDefinition;

public record FeedDefinitionAzBlob
{
    public AzBlobContainerUri Feed { get; }
    public AzBlobUri Package { get; }
    public AzBlobUri PackageGz { get; }
    public AzBlobUri PackageStamps { get; }

    public static Result<FeedDefinitionAzBlob> Of(AzBlobFeedUri feedUri)
    {
        var feed = AzBlobContainerUri.Of(feedUri.Full);
        var package = AzBlobUri.Create($"{feedUri.BaseUri}/Packages{feedUri.Query}");
        var packageGz = AzBlobUri.Create($"{feedUri.BaseUri}/Packages.gz{feedUri.Query}");
        var packageStamps = AzBlobUri.Create($"{feedUri.BaseUri}/Packages.stamps{feedUri.Query}");
        return Result
            .Combine(feed, package, packageGz, packageStamps)
            .Map(() => new FeedDefinitionAzBlob(feed.Value, package.Value, packageGz.Value, packageStamps.Value));
    }
    private FeedDefinitionAzBlob(AzBlobContainerUri feed, AzBlobUri package, AzBlobUri packageGz, AzBlobUri packageStamps)
    {
        Feed = feed;
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
    }
}

