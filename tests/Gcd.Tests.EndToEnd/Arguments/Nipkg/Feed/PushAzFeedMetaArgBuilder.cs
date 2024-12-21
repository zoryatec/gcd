using static Gcd.Contract.Nipkg.PushAzBlobFeedMetaData;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Feed;

public class PushAzFeedMetaArgBuilder : ArgumentsBuilder
{
    public PushAzFeedMetaArgBuilder()
    {
        WithArg("nipkg");
        WithArg("feed");
        WithArg(COMMAND_NAME);
    }

    public PushAzFeedMetaArgBuilder WithFeedLocalPath(string feedLocalPath)
    {
        WithOption(LOCAL_FEED_PATH_OPTION, feedLocalPath);
        return this;
    }

    public PushAzFeedMetaArgBuilder WithFeedUri(string feedUri)
    {
        WithOption(REMOTE_FEED_URI_OPTION, feedUri);
        return this;
    }
}
