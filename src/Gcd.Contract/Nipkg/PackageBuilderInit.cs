using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public class PackageBuilderInit
    {
        public const string COMMAND = "create";
        public const string COMMAND_DESCRIPTION = "Create package template";

        public const string PACKAGE_PATH_OPTION = "--package-path";
        public const string PACKAGE_PATH_DESCRIPTION = "Path to a package";

        public const string PACKAGE_NAME_OPTION = "--package-name";
        public const string PACKAGE_NAME_DESCRIPTION = "Package name";

        public const string PACKAGE_VERSION_OPTION = "--package-version";
        public const string PACKAGE_VERSION_DESCRIPTION = "version of the package";

        public const string PACKAGE_DESTINATION_DIR_OPTION = "--package-destination-dir";
        public const string PACKAGE_DESTINATION_DIR_DESCRIPTION = "path where package will be placed";

        public const string SUCESS_MESSAGE = "Package initialised successully";
    }
}