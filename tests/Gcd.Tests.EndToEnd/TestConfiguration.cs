using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class TestConfiguration
    {
        public string GetAzureFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/nipkg-private-feed?sp=racwdl&st=2024-12-07T14:09:45Z&se=2024-12-07T22:09:45Z&spr=https&sv=2022-11-02&sr=c&sig=nDgua7VeaaKRnkLMda9C%2F5FXurPPWWYQaNHFXuw0XqA%3D";
        public string GetAzurePublicFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePushTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePullTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/pull-test-feed";
        public string GetAzureAddPkgTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/add-pkg-test-feed?sp=racwdl&st=2024-12-07T14:07:21Z&se=2024-12-07T22:07:21Z&spr=https&sv=2022-11-02&sr=c&sig=U2Eg0SV8HIQoEniIw8nSAoySYlBy%2BlKcLI%2B1J5gyaic%3D";

    }
}
