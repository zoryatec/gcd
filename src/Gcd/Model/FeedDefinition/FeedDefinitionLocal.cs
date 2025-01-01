using CSharpFunctionalExtensions;
using Gcd.Model.File;

namespace Gcd.Model.FeedDefinition;

public record FeedDefinitionLocal
{
    public LocalDirPath Feed { get; }
    public LocalFilePath Package { get; }
    public LocalFilePath PackageGz { get; }
    public LocalFilePath PackageStamps { get; }

    //IFileDescriptor IFeedDefinition.Package => Package;
    //IFileDescriptor IFeedDefinition.PackageGz => PackageGz;
    //IFileDescriptor IFeedDefinition.PackageStamps => PackageStamps;
    //IDirectoryDescriptor IFeedDefinition.Feed => Feed;

    public static Result<FeedDefinitionLocal> Of(LocalDirPath feedDirPath)
    {
        var package = LocalFilePath.Offf($"{feedDirPath.Value}\\Packages");
        var packageGz = LocalFilePath.Offf($"{feedDirPath.Value}\\Packages.gz");
        var packageStamps = LocalFilePath.Offf($"{feedDirPath.Value}\\Packages.stamps");
        return Result
            .Combine(package, packageGz, packageStamps)
            .Map(() => new FeedDefinitionLocal(feedDirPath, package.Value, packageGz.Value, packageStamps.Value));
    }
    private FeedDefinitionLocal(LocalDirPath feed, LocalFilePath package, LocalFilePath packageGz, LocalFilePath packageStamps)
    {
        Feed = feed;
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
    }
}

