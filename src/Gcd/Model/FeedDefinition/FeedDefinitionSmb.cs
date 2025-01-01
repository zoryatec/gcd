using CSharpFunctionalExtensions;
using Gcd.Model.File;
using Gcd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.FeedDefinition;

public record FeedDefinitionSmb : IFeedDefinition
{
    public SmbDir Feed { get; }
    public SmbPath Package { get; }
    public SmbPath PackageGz { get; }
    public SmbPath PackageStamps { get; }

    IDirectoryDescriptor IFeedDefinition.Feed => Feed;
    IFileDescriptor IFeedDefinition.Package => Package;
    IFileDescriptor IFeedDefinition.PackageGz => PackageGz;
    IFileDescriptor IFeedDefinition.PackageStamps => PackageStamps;

    public static Result<FeedDefinitionSmb> Of(AzBlobFeedUri feedUri)
    {
        var feed = SmbDir.Of(feedUri.Full);
        var package = SmbPath.Of($"{feedUri.BaseUri}/Packages{feedUri.Query}");
        var packageGz = SmbPath.Of($"{feedUri.BaseUri}/Packages.gz{feedUri.Query}");
        var packageStamps = SmbPath.Of($"{feedUri.BaseUri}/Packages.stamps{feedUri.Query}");
        return Result
            .Combine(feed, package, packageGz, packageStamps)
            .Map(() => new FeedDefinitionSmb(feed.Value, package.Value, packageGz.Value, packageStamps.Value));
    }
    private FeedDefinitionSmb(SmbDir feed, SmbPath package, SmbPath packageGz, SmbPath packageStamps)
    {
        Feed = feed;
        Package = package;
        PackageGz = packageGz;
        PackageStamps = packageStamps;
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
