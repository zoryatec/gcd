﻿namespace Gcd.Tests.EndToEnd.Arguments.Nipkg
{
    public class PullFeedMetaArgBuilder : ArgumentsBuilder
    {
        public PullFeedMetaArgBuilder()
        {
            WithArg("nipkg");
            WithArg("pull-feed-meta");
        }

        public PullFeedMetaArgBuilder WithFeedLocalPath(string feedLocalPath)
        {
            WithOption("--feed-local-path",feedLocalPath);
            return this;
        }

        public PullFeedMetaArgBuilder WithFeedUri(string feedUri)
        {
            WithOption("--feed-uri", feedUri);
            return this;
        }
    }
}