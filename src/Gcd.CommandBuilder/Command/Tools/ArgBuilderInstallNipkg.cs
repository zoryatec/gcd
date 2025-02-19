using Gcd.Commands.Tools;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Tools
{
    public class ArgBuilderInstallNipkg :  ArgBuilder
    {
        public ArgBuilderInstallNipkg(Arguments arguments) : base(arguments)
        {
            WithArg(UseCmdInstallNipkgExt.NAME);
        }
        
        public ArgBuilderInstallNipkg WithInstallerSourceUri(string installerSourceUri)
        {
            WithOption(NipkgInstallerSourceUrlOption.NAME, installerSourceUri);
            return this;
        }
    }
}
