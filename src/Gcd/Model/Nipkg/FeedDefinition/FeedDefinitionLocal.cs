using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;

namespace Gcd.Model.Nipkg.FeedDefinition;

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
        var result =
            from package in LocalFilePath.Of($"{feedDirPath.Value}\\Packages")
            from packageGz in LocalFilePath.Of($"{feedDirPath.Value}\\Packages.gz")
            from packageStamps in LocalFilePath.Of($"{feedDirPath.Value}\\Packages.stamps")
            select new FeedDefinitionLocal(
                feedDirPath,
                package,
                packageGz,
                packageStamps);

        return result.MapError(er => er.Message);
    }
    private FeedDefinitionLocal(LocalDirPath feed, LocalFilePath package, LocalFilePath packageGz, LocalFilePath packageStamps)
    {
        Feed = feed;
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
    }
}

