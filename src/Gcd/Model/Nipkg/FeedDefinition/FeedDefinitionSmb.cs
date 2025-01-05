using CSharpFunctionalExtensions;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.FeedDefinition;
using Gcd.Common;

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
        //var feed = SmbDirPath.Of(smbaddress.Value);
        //var package = SmbFilePath.Of($"{smbaddress.Value}\\Packages");
        //var packageGz = SmbFilePath.Of($"{smbaddress.Value}\\Packages.gz");
        //var packageStamps = SmbFilePath.Of($"{smbaddress.Value}\\Packages.stamps");
        //var result = Result
        //    .Combine(package, packageGz, packageStamps);

        //var rest = feed
        //    .Bind((f) => package)
        //    .Bind((p) => packageGz)
        //    .Bind((pgz) => packageStamps)
        //    .Map(ps => new FeedDefinitionSmb(feed.Value, package.Value, packageGz.Value, packageStamps.Value, smbaddress, smbPassword, smbUserName));

        //            return feed
        //    .Map((d) => new FeedDefinitionSmb(feed.Value, package.Value, packageGz.Value, packageStamps.Value, smbaddress, smbPassword, smbUserName))
        //    .MapError(x => x.Message);


        var result =
            from feed in SmbDirPath.Of(smbaddress.Value)
            from package in SmbFilePath.Of($"{smbaddress.Value}\\Packages")
            from packageGz in SmbFilePath.Of($"{smbaddress.Value}\\Packages.gz")
            from packageStamps in SmbFilePath.Of($"{smbaddress.Value}\\Packages.stamps")
            select new FeedDefinitionSmb(
                feed,
                package,
                packageGz,
                packageStamps,
                smbaddress,
                smbPassword,
                smbUserName);

        return result.MapError(x => x.Message);
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

