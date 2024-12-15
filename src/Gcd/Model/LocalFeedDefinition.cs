using CSharpFunctionalExtensions;

namespace Gcd.Model;

public record LocalFeedDefinition
{
    public LocalDirPath Feed { get; }
    public LocalFilePath Package { get; }
    public LocalFilePath PackageGz { get; }
    public LocalFilePath PackageStamps { get; }

    public static Result<LocalFeedDefinition> Of(LocalDirPath feedDirPath)
    {
        var package = LocalFilePath.Offf($"{feedDirPath.Value}\\Packages");
        var packageGz = LocalFilePath.Offf($"{feedDirPath.Value}\\Packages.gz");
        var packageStamps = LocalFilePath.Offf($"{feedDirPath.Value}\\Packages.stamps");
        return Result
            .Combine(package, packageGz, packageStamps)
            .Map(() => new LocalFeedDefinition(feedDirPath, package.Value, packageGz.Value, packageStamps.Value));
    }
    private LocalFeedDefinition(LocalDirPath feed, LocalFilePath package, LocalFilePath packageGz, LocalFilePath packageStamps)
    {
        Feed = feed;
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
    }
}

