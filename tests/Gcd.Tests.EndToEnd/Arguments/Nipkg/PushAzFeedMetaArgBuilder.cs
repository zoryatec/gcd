namespace Gcd.Tests.EndToEnd.Arguments.Nipkg
{
    public class PushAzFeedMetaArgBuilder : ArgumentsBuilder
    {
        public PushAzFeedMetaArgBuilder()
        {
            WithArg("nipkg");
            WithArg("push-feed-meta");
        }

        public PushAzFeedMetaArgBuilder WithFeedLocalPath(string feedLocalPath)
        {
            WithOption("--feed-local-path",feedLocalPath);
            return this;
        }

        public PushAzFeedMetaArgBuilder WithFeedUri(string feedUri)
        {
            WithOption("--feed-uri", feedUri);
            return this;
        }
    }
}