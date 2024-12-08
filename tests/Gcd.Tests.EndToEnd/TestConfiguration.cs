using Castle.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string GetAzureFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/nipkg-private-feed?sp=racwdl&st=2024-12-07T22:27:23Z&se=2024-12-08T06:27:23Z&spr=https&sv=2022-11-02&sr=c&sig=oRgLarNu2U8BrvHvxPCPX8SfrnQo2IJuMwcvIPCdXS0%3D";
        public string GetAzurePublicFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePushTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePullTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/pull-test-feed";
        public string GetAzureAddPkgTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/add-pkg-test-feed?sp=racwdl&st=2024-12-07T22:29:00Z&se=2024-12-08T06:29:00Z&spr=https&sv=2022-11-02&sr=c&sig=WxYbmpzBNY7A0lcGkxpKV%2B05YYcznkxNi02ClQB%2FIh4%3D";

    }
}
