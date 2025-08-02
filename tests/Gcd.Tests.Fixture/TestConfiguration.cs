using Microsoft.Extensions.Configuration;

namespace Gcd.Tests.Fixture
{
    public class TestConfiguration
    {
        private readonly IConfiguration _config;

        public TestConfiguration(IConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(config, nameof(config));
            _config = config;
            ArgumentNullException.ThrowIfNull(_config, nameof(_config));
        }
        //public int MyProperty { get; set; }
        public string GetAzureFeedUri() => _config["AzureFeedUri"] ?? throw new ArgumentNullException("_config[\"AzureFeedUri\"]");
        public string GetAzurePublicFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePushTestFeedUri() => "https://zoryatecartifacts.blob.core.windows.net/gcd-feed";
        public string GetAzurePullTestFeedUri() => "https://stztgcdtest.blob.core.windows.net/feed-test-pull";
        public string GetAzureAddPkgTestFeedUri() => _config["AzureAddPkgTestFeedUri"] ?? throw new ArgumentNullException("_config[\"AzureAddPkgTestFeedUri\"]");
        public string GetAzurePushPullTestFeedUri() => _config["AzurePushPullTestFeedUri"] ?? throw new ArgumentNullException("_config[\"AzurePushPullTestFeedUri\"]");
        public string GetGitRepoAddress() => _config["GitRepoAddress"] ?? throw new ArgumentNullException("_config[\"GitRepoAddress\"]");
        public string GetGitPassword() => _config["GitPassword"] ?? throw new ArgumentNullException("_config[\"GitPassword\"] ");
        public string GetGitUserName() => _config["GitUserName"] ?? throw new ArgumentNullException(" _config[\"GitUserName\"]");
        public string GetSmbRepoAddress() => _config["SmbShareAddress"] ?? throw new ArgumentNullException("_config[\"SmbShareAddress\"]");
        public string GetSmbPassword() => _config["SmbUserPass"] ?? throw new ArgumentNullException("_config[\"SmbUserPass\"] ");
        public string GetSmbUserName() => _config["SmbUserName"] ?? throw new ArgumentNullException(" _config[\"SmbUserName\"]");

    }
}
