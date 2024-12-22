using static Gcd.Contract.Nipkg.PackageBuilderInit;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class PackageBuilderInitArgBuilder : ArgumentsBuilder
{
    public PackageBuilderInitArgBuilder()
    {
        WithArg("nipkg");
        WithArg("builder");
        WithArg(COMMAND);
    }

    public PackageBuilderInitArgBuilder WithPackageBuilderDirectory(string value)
    {
        WithOption(PACKAGE_BUILDER_DIR_OPTION, value);
        return this;
    }
    public PackageBuilderInitArgBuilder WithPackageName(string value)
    {
        WithOption(PACKAGE_NAME_OPTION, value);
        return this;
    }

    public PackageBuilderInitArgBuilder WithPackageVersion(string value)
    {
        WithOption(PACKAGE_VERSION_OPTION, value);
        return this;
    }


    //public PackageBuilderInitArgBuilder WithPackageBuilderInstalationDir(string value)
    //{
    //    WithOption(PACKAGE_DESTINATION_DIR_OPTION, value);
    //    return this;
    //}
}
