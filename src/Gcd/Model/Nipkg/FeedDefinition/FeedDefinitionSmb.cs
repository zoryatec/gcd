using CSharpFunctionalExtensions;
using Gcd.Model.FeedDefinition;
using Gcd.Model.File;

namespace Gcd.Model.Nipkg.FeedDefinition;

public record FeedDefinitionSmb : IFeedDefinition
{
    public SmbShareAddress SmbShareAddress { get; }
    public SmbUserName SmbUserName { get; }
    public SmbPassword SmbPassword { get; }
    public SmbDirPath Feed { get; }
    public SmbFilePath Package { get; }
    public SmbFilePath PackageGz { get; }
    public SmbFilePath PackageStamps { get; }

    IDirectoryDescriptor IFeedDefinition.Feed => Feed;
    IFileDescriptor IFeedDefinition.Package => Package;
    IFileDescriptor IFeedDefinition.PackageGz => PackageGz;
    IFileDescriptor IFeedDefinition.PackageStamps => PackageStamps;

    public static Result<FeedDefinitionSmb> Of(SmbShareAddress smbaddress, SmbUserName smbUserName, SmbPassword smbPassword)
    {
        var feed = SmbDirPath.Of(smbaddress.Value);
        var package = SmbFilePath.Of($"{smbaddress.Value}\\Packages");
        var packageGz = SmbFilePath.Of($"{smbaddress.Value}\\Packages.gz");
        var packageStamps = SmbFilePath.Of($"{smbaddress.Value}\\Packages.stamps");
        return Result
            .Combine(feed, package, packageGz, packageStamps)
            .Map(() => new FeedDefinitionSmb(feed.Value, package.Value, packageGz.Value, packageStamps.Value, smbaddress, smbPassword, smbUserName));
    }
    private FeedDefinitionSmb(SmbDirPath feed, SmbFilePath package, SmbFilePath packageGz, SmbFilePath packageStamps, SmbShareAddress shareAddress, SmbPassword smbPassword, SmbUserName smbUserName)
    {
        Feed = feed;
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
        SmbPassword = smbPassword;
        SmbUserName = smbUserName;
        SmbShareAddress = shareAddress;
    }
}

