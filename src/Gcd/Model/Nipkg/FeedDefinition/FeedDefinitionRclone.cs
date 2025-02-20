using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.RemoteFileSystem.Rclone.Abstractions;

namespace Gcd.Model.Nipkg.FeedDefinition;

public record FeedDefinitionRclone : IFeedDefinition
{
    public RcloneDirPath Feed { get; }
    public RcloneFilePath Package { get; }
    public RcloneFilePath PackageGz { get; }
    public RcloneFilePath PackageStamps { get; }
    
    IDirectoryDescriptor IFeedDefinition.Feed => Feed;
    IFileDescriptor IFeedDefinition.Package => Package;
    IFileDescriptor IFeedDefinition.PackageGz => PackageGz;
    IFileDescriptor IFeedDefinition.PackageStamps => PackageStamps;
    
    public static Result<FeedDefinitionRclone> Of(RcloneDirPath feedDirPath)
    {
        var result =
            from package in RcloneFilePath.Of($"{feedDirPath.Value}/Packages")
            from packageGz in RcloneFilePath.Of($"{feedDirPath.Value}/Packages.gz")
            from packageStamps in RcloneFilePath.Of($"{feedDirPath.Value}/Packages.stamps")
            select new FeedDefinitionRclone(
                feedDirPath,
                package,
                packageGz,
                packageStamps);

        return result.MapError(er => er.Message);
    }
    private FeedDefinitionRclone(RcloneDirPath feed, RcloneFilePath package, RcloneFilePath packageGz, RcloneFilePath packageStamps)
    {
        Feed = feed;
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
    }
}

