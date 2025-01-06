using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Services;

namespace Gcd.Model.Nipkg.FeedDefinition;

public record FeedDefinitionAzBlob : IFeedDefinition
{
    public AzBlobContainerUri Feed { get; }
    public AzBlobUri Package { get; }
    public AzBlobUri PackageGz { get; }
    public AzBlobUri PackageStamps { get; }

    IDirectoryDescriptor IFeedDefinition.Feed => Feed;
    IFileDescriptor IFeedDefinition.Package => Package;
    IFileDescriptor IFeedDefinition.PackageGz => PackageGz;
    IFileDescriptor IFeedDefinition.PackageStamps => PackageStamps;

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

