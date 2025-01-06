using Gcd.Commands.Nipkg.Builder;
using Gcd.Commands.Nipkg;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class PackageBuilderInitArgBuilder : ArgumentsBuilder
{
    public PackageBuilderInitArgBuilder()
    {
        WithArg(UseMenuNipkgExtension.NAME);
        WithArg(UseMenuBuilderExt.NAME);
        WithArg(UseCmdInitExt.NAME);
    }

    public PackageBuilderInitArgBuilder WithPackageBuilderDirectory(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }
    public PackageBuilderInitArgBuilder WithPackageName(string value)
    {
        WithOption(PackageNameOption.NAME, value);
        return this;
    }

    public PackageBuilderInitArgBuilder WithPackageVersion(string value)
    {
        WithOption(PackageVersionOption.NAME, value);
        return this;
    }


    //public PackageBuilderInitArgBuilder WithPackageBuilderInstalationDir(string value)
    //{
    //    WithOption(PACKAGE_DESTINATION_DIR_OPTION, value);
    //    return this;
    //}
}
