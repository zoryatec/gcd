﻿using static Gcd.Contract.Nipkg.DownloadNipkg;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg
{
    public class DownloadNipkgArgBuilder : ArgumentsBuilder
    {
        public DownloadNipkgArgBuilder()
        {
            WithArg("nipkg");
            WithArg(COMMAND);
        }

        public DownloadNipkgArgBuilder WithLocalPath(string fileLocalPath)
        {
            WithOption(DOWNLOAD_PATH_OPTION, fileLocalPath);
            return this;
        }
    }
}