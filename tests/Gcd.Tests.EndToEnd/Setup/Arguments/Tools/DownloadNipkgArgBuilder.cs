using Gcd.Commands.Tools;
using static Gcd.Contract.Nipkg.DownloadNipkg;

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
            WithOption(DOWNLOAD_PATH_OPTION, fileLocalPath);
            return this;
        }
    }
}
