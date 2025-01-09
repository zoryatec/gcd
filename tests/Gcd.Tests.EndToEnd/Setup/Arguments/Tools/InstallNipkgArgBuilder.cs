using Gcd.Commands.Tools;

namespace Gcd.Tests.EndToEnd.Arguments.Tools
{
    public class InstallNipkgArgBuilder : ArgumentsBuilder
    {
        public InstallNipkgArgBuilder()
        {
            WithArg(UseMenuToolsExt.NAME);
            WithArg(UseCmdInstallNipkgExt.NAME);
        }
        
        public InstallNipkgArgBuilder WithInstallerSourceUri(string installerSourceUri)
        {
            WithOption(NipkgInstallerSourceUrlOption.NAME, installerSourceUri);
            return this;
        }
    }
}
