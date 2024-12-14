using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Nipkg
{
    public class PackageBuilderSetProperty
    {
        public const string COMMAND = "set-property";
        public const string COMMAND_DESCRIPTION = "set version package builder directory";

        public const string PACKAGE_PATH_OPTION = "--package-path";
        public const string PACKAGE_PATH_DESCRIPTION = "Path to a package";

        public const string PACKAGE_ARCHITECTURE_OPTION = "--package-architecture";
        public const string PACKAGE_ARCHITECTURE_DESCRIPTION = "";

        public const string PACKAGE_HOME_PAGE_OPTION = "--package-home-page";
        public const string PACKAGE_HOME_PAGE_DESCRIPTION = "package home page to set";

        public const string PACKAGE_MAINTAINER_OPTION = "--package-maintainer";
        public const string PACKAGE_MAINTAINER_DESCRIPTION = "";

        public const string PACKAGE_DESCRIPTION_OPTION = "--package-description";
        public const string PACKAGE_DESCRIPTION_DESCRIPTION = "";

        public const string PACKAGE_XB_PLUGIN_OPTION = "--package-xb-plugin";
        public const string PACKAGE_XB_PLUGIN_DESCRIPTION = "";

        public const string PACKAGE_XB_USER_VISIBLE_OPTION = "--package-xb-user-visible";
        public const string PACKAGE_XB_USER_VISIBLE_DESCRIPTION = "";

        public const string PACKAGE_XB_STORE_PRODUCT_OPTION = "--package-xb-store-product";
        public const string PACKAGE_XB_STORE_PRODUCT_DESCRIPTION = "";

        public const string PACKAGE_XB_SECTION_OPTION = "--package-xb-section";
        public const string PACKAGE_XB_SECTION_DESCRIPTION = "";

        public const string PACKAGE_NAME_OPTION = "--package-name";
        public const string PACKAGE_NAME_DESCRIPTION = "";

        public const string PACKAGE_VERSION_OPTION = "--package-version";
        public const string PACKAGE_VERSION_DESCRIPTION = "package version to set";

        public const string PACKAGE_DEPENDENCIES_OPTION = "--package-dependencies";
        public const string PACKAGE_DEPENDENCIES_DESCRIPTION = "";

        public const string SUCESS_MESSAGE = "Package property set sucessfuly successully";
    }
}

