using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public static class PackageBuild
    {
        public const string COMMAND = "build";
        public const string COMMAND_DESCRIPTION = "Pull metadata files from remote feed to local";

        public const string PACKAGE_CONTENT_DIR_OPTION = "--package-sourec-dir";
        public const string PACKAGE_CONTENT_DIR_DESCRIPTION = "Directory with content of the package";

        public const string PACKAGE_NAME_OPTION = "--package-name";
        public const string PACKAGE_NAME_DESCRIPTION = "Package name";

        public const string PACKAGE_VERSION_OPTION = "--package-version";
        public const string PACKAGE_VERSION_DESCRIPTION = "version of the package";

        public const string PACKAGE_INSTALATION_DIR_OPTION = "--package-instalation-dir";
        public const string PACKAGE_INSTALATION_DIR_DESCRIPTION = "path where package will be installed";

        public const string PACKAGE_DESTINATION_DIR_OPTION = "--package-destination-dir";
        public const string PACKAGE_DESTINATION_DIR_DESCRIPTION = "path where package will be placed";

        public const string SUCESS_MESSAGE = "Package built successully";
    }
}

