using Gcd.Commands.Tools;

namespace Gcd.Tests.EndToEnd.Arguments.Tools
{
    public class DownloadNipkgArgBuilder : ArgumentsBuilder
    {
        public DownloadNipkgArgBuilder()
        {
            WithArg(UseMenuToolsExt.NAME);
            WithArg(UseCmdDownloadNipkgExt.NAME);
        }

        public DownloadNipkgArgBuilder WithLocalPath(string fileLocalPath)
        {
            WithOption(UseCmdDownloadNipkgExt.NAME, fileLocalPath);
            return this;
        }
    }
}
