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
    SmbShareAddress SmbShareAddress { get; }
    SmbUserName SmbUserName { get; }
    SmbPassword SmbPassword { get; }
    public SmbDir Feed { get; }
    public SmbPath Package { get; }
    public SmbPath PackageGz { get; }
    public SmbPath PackageStamps { get; }

    IDirectoryDescriptor IFeedDefinition.Feed => Feed;
    IFileDescriptor IFeedDefinition.Package => Package;
    IFileDescriptor IFeedDefinition.PackageGz => PackageGz;
    IFileDescriptor IFeedDefinition.PackageStamps => PackageStamps;

    public static Result<FeedDefinitionSmb> Of(SmbShareAddress smbaddress, SmbUserName smbUserName, SmbPassword smbPassword)
    {
        var feed = SmbDir.Of(smbaddress.Value);
        var package = SmbPath.Of($"{smbaddress.Value}\\Packages");
        var packageGz = SmbPath.Of($"{smbaddress.Value}\\Packages.gz");
        var packageStamps = SmbPath.Of($"{smbaddress.Value}\\Packages.stamps");
        return Result
            .Combine(feed, package, packageGz, packageStamps)
            .Map(() => new FeedDefinitionSmb(feed.Value, package.Value, packageGz.Value, packageStamps.Value,smbaddress,smbPassword,smbUserName));
    }
    private FeedDefinitionSmb(SmbDir feed, SmbPath package, SmbPath packageGz, SmbPath packageStamps, SmbShareAddress shareAddress, SmbPassword smbPassword, SmbUserName smbUserName)
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


public record SmbPath : IFileDescriptor
{
    public static Result<SmbPath> Of(Maybe<string> maybeBlobUri)
    {
        return maybeBlobUri.ToResult("FeedUri should not be empty")
        .Ensure(blobUri => blobUri != string.Empty, "FeedUri should not be empty")
        .MapTry((blobUri) => new Uri(blobUri), ex => ex.Message)
        .Map(blobUri => new SmbPath(blobUri));
    }
    private SmbPath(Uri value) => _uri = value;
    private Uri _uri;
    public string Value { get => _uri.AbsoluteUri; }
}


public record SmbDir : IDirectoryDescriptor
{
    public static Result<SmbDir> Of(Maybe<string> feedUriOrNothing)
    {
        return feedUriOrNothing.ToResult("FeedUri should not be empty")
            .Ensure(feedUri => feedUri != string.Empty, "FeedUri should not be empty")
            .MapTry((uri) => new Uri(uri), ex => ex.Message)
            .Map(feedUri => new SmbDir(feedUri));
    }
    private SmbDir(Uri value) => _uri = value;
    private Uri _uri;
    public string Full { get => _uri.AbsoluteUri; }
    public string BaseUri { get => _uri.GetLeftPart(UriPartial.Path); }
    public string Query { get => _uri.Query; }
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
        return UserName.ToResult($"{nameof(GitRepoAddress)} cannot be empty")
            .Map(x => new SmbShareAddress(x));
    }
    private SmbShareAddress(string value) => Value = value;
    public string Value { get; }
}



