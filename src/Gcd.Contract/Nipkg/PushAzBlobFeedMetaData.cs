using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public static class PushAzBlobFeedMetaData
    {
        public const string COMMAND_NAME = "push-feed-meta-az";
        public const string COMMAND_DESCRIPTION = "push-feed-meta";

        public const string LOCAL_FEED_PATH_OPTION = "--feed-local-path";
        public const string LOCAL_FEED_PATH_OPTION_DESCRIPTION = "Path to local directory with feed";

        public const string REMOTE_FEED_URI_OPTION = "--feed-uri";
        public const string REMOTE_FEED_PATH_OPTION_DESCRIPTION = "Uri to reomte az blob feed where meta data will be pushed";

        public const string SUCESS_MESSAGE = "Metadata pushed successully";
    }
}
