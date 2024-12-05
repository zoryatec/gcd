using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class TestConfiguration
    {
        public string GetAzureFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/nipkg-private-feed?sp=racwdl&st=2024-12-04T21:54:19Z&se=2024-12-05T05:54:19Z&spr=https&sv=2022-11-02&sr=c&sig=2lfHgt5HxVvz0VKyhII0IMTzUlf1lSYs9zL1MDwJZ3w%3D";
        public string GetAzurePublicFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePushTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePullTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/pull-test-feed";
        public string GetAzureAddPkgTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/add-pkg-test-feed?sp=racwdl&st=2024-12-05T21:37:17Z&se=2024-12-06T05:37:17Z&spr=https&sv=2022-11-02&sr=c&sig=eT4Pnbhbrj2C1SWG9dc2UKJr3833owUxAespWJktjfs%3D";

    }
}
