using Gcd.Commands.Tools;
using static Gcd.Contract.Nipkg.InstallNipkg;

namespace Gcd.Tests.EndToEnd.Arguments.Tools
{
    public class InstallNipkgArgBuilder : ArgumentsBuilder
    {
        public InstallNipkgArgBuilder()
        {
            WithArg(UseMenuToolsExt.NAME);
            WithArg(UseCmdInstallNipkgExt.NAME);
        }
    }
}
