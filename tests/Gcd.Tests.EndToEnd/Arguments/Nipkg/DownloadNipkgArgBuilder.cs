using System;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg
{
    public class DownloadNipkgArgBuilder : ArgumentsBuilder
    {
        public DownloadNipkgArgBuilder()
        {
            WithArg("nipkg");
            WithArg("download-nipkg");
        }

        public DownloadNipkgArgBuilder WithLocalPath(string fileLocalPath)
        {
            WithOption("--download-path", fileLocalPath);
            return this;
        }
    }
}
