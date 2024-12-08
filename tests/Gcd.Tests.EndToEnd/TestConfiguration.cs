using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Gcd.Tests.EndToEnd
{
    public class TestConfiguration
    {
        private readonly IConfiguration _config;

        public TestConfiguration(IConfiguration config)
        {
            _config = config;
        }
        //public int MyProperty { get; set; }
        public string GetAzureFeedUri() => _config["AzureFeedUri"];
        public string GetAzurePublicFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePushTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePullTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/pull-test-feed";
        public string GetAzureAddPkgTestFeedUri() => _config["AzureAddPkgTestFeedUri"];
        public string GetAzurePushPullTestFeedUri() => _config["AzurePushPullTestFeedUri"];

    }
}
