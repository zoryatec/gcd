using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public static class AddPackageToAzFeed
    {
        public const string AZ_FEED_URI_OPTION = "--feed-url";
        public const string AZ_FEED_URI_OPTION_DESCRIPTION = "Uri to reomte az blob feed where meta data will be pushed";

        public const string SUCESS_MESSAGE = "Package added successully";
    }
}
