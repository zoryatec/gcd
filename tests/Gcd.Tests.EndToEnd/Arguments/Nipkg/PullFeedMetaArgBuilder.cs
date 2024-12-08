using static Gcd.Contract.Nipkg.PullFeedMetaData;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg
{
    public class PullFeedMetaArgBuilder : ArgumentsBuilder
    {
        public PullFeedMetaArgBuilder()
        {
            WithArg("nipkg");
            WithArg(COMMAND);
        }

        public PullFeedMetaArgBuilder WithFeedLocalPath(string feedLocalPath)
        {
            WithOption(FEED_LOCAL_PATH_OPTION,feedLocalPath);
            return this;
        }

        public PullFeedMetaArgBuilder WithFeedUri(string feedUri)
        {
            WithOption(REMOTE_FEED_URI_OPTION, feedUri);
            return this;
        }
    }
}
