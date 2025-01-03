using CSharpFunctionalExtensions;
using Gcd.Model.File;
using Gcd.Services;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.FeedDefinition;

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
            .Map(() => new FeedDefinitionSmb(feed.Value, package.Value, packageGz.Value, packageStamps.Value,smbaddress,smbPassword,smbUserName));
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


public record SmbFilePath : IFileDescriptor
{
    public static Result<SmbFilePath> Of(Maybe<string> maybeBlobUri)
    {
        return maybeBlobUri.ToResult("FeedUri should not be empty")
        .Ensure(blobUri => blobUri != string.Empty, "FeedUri should not be empty")
        .Map(blobUri => new SmbFilePath(blobUri));
    }
    private SmbFilePath(string value) => Value = value;

    public string Value { get; }
}


public record SmbDirPath : IDirectoryDescriptor
{
    public static Result<SmbDirPath> Of(Maybe<string> feedUriOrNothing)
    {
        return feedUriOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "FeedUri should not be empty")
            .Map(feedUri => new SmbDirPath(feedUri));
    }
    private SmbDirPath(string value) => Value = value;

    public string Value { get; }
}


public record SmbUserName
{
    public static Result<SmbUserName> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(SmbUserName)} cannot be empty")
            .Map(x => new SmbUserName(x));
    }
    public SmbUserName(string value) => Value = value;
    public string Value { get; }
}

public record SmbPassword
{
    public static Result<SmbPassword> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(SmbPassword)} cannot be empty")
            .Map(x => new SmbPassword(x));
    }
    private SmbPassword(string value) => Value = value;
    public string Value { get; }
}

public record SmbShareAddress
{
    public static Result<SmbShareAddress> Of(Maybe<string> UserName)
    {
        return UserName.ToResult($"{nameof(SmbShareAddress)} cannot be empty")
            .Map(x => new SmbShareAddress(x));
    }
    private SmbShareAddress(string value) => Value = value;
    public string Value { get; }
}



