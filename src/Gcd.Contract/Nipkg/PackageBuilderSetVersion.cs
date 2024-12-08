using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public class PackageBuilderSetVersion
    {
        public const string COMMAND = "set-version";
        public const string COMMAND_DESCRIPTION = "set version package builder directory";

        public const string PACKAGE_PATH_OPTION = "--package-path";
        public const string PACKAGE_PATH_DESCRIPTION = "Path to a package";

        public const string PACKAGE_VERSION_OPTION = "--package-version";
        public const string PACKAGE_VERSION_DESCRIPTION = "package version to set";

        public const string SUCESS_MESSAGE = "Package version set sucessfuly successully";
    }
}

