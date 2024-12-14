using static Gcd.Contract.Nipkg.InstallNipkg;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg
{
    public class InstallNipkgArgBuilder : ArgumentsBuilder
    {
        public InstallNipkgArgBuilder()
        {
            WithArg("nipkg");
            WithArg(COMMAND);
        }
    }
}
