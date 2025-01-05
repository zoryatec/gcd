using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.FeedAzBlob;
using Gcd.Commands.Nipkg.FeedGit;
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
        WithOption(AzFeedUrlOption.NAME, feedUri);
        return this;
    }
}
