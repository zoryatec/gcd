using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.FeedAzBlob;
using Gcd.Commands.Nipkg.FeedGit;
using static Gcd.Contract.Nipkg.PushAzBlobFeedMetaData;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;

public class PushAzFeedMetaArgBuilder : ArgumentsBuilder
{
    public PushAzFeedMetaArgBuilder()
    {
        WithArg(UseMenuNipkgExtension.NAME);
        WithArg(UseMenuFeedAzBlobExt.NAME);
        WithArg(UseCmdPushFeedMetaExt.NAME);
    }

    public PushAzFeedMetaArgBuilder WithFeedLocalPath(string feedLocalPath)
    {
        WithOption(FeedLocalDirOption.NAME, feedLocalPath);
        return this;
    }

    public PushAzFeedMetaArgBuilder WithFeedUri(string feedUri)
    {
        WithOption(REMOTE_FEED_URI_OPTION, feedUri);
        return this;
    }
}
