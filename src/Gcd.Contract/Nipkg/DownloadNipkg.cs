using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public static class DownloadNipkg
    {
        public const string COMMAND = "download-nipkg";
        public const string COMMAND_DESCRIPTION = "Downloads nipkg installer";

        public const string DOWNLOAD_PATH_OPTION = "--download-path";
        public const string DOWNLOAD_PATH_DESCRIPTION = "File path must end with exe";

        public const string SUCESS_MESSAGE = "Nipkg download succesfully pushed successully";
    }
}