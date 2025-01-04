using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.FeedAzBlob;
using Gcd.Commands.Nipkg.FeedGit;
using static Gcd.Contract.Nipkg.PullFeedMetaData;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;

public class PullFeedMetaArgBuilder : ArgumentsBuilder
{
    public PullFeedMetaArgBuilder()
    {
        WithArg(UseMenuNipkgExtension.NAME);
        WithArg(UseMenuFeedAzBlobExt.NAME);
        WithArg(UseCmdPullFeedMetaExt.NAME);
    }

    public PullFeedMetaArgBuilder WithFeedLocalPath(string feedLocalPath)
    {
        WithOption(FeedLocalDirOption.NAME, feedLocalPath);
        return this;
    }

    public PullFeedMetaArgBuilder WithFeedUri(string feedUri)
    {
        WithOption(REMOTE_FEED_URI_OPTION, feedUri);
        return this;
    }
}

