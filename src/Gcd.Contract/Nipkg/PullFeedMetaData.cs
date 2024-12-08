using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public static class PullFeedMetaData
    {
        public const string COMMAND = "pull-feed-meta";
        public const string COMMAND_DESCRIPTION = "Pull metadata files from remote feed to local";

        public const string FEED_LOCAL_PATH_OPTION = "--feed-local-path";
        public const string FEED_LOCAL_PATH_DESCRIPTION = "--feed-local-path";

        public const string REMOTE_FEED_URI_OPTION = "--feed-uri";
        public const string REMOTE_FEED_URI_DESCRIPTION = "--feed-local-path";

        public const string SUCESS_MESSAGE = "Metadata pulled successully";

    }
}
