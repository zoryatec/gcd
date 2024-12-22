using static Gcd.Contract.Nipkg.PackageBuild;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class PackageBuildArgBuilder : ArgumentsBuilder
{
    public PackageBuildArgBuilder()
    {
        WithArg("nipkg");
        WithArg(COMMAND);
    }

    public PackageBuildArgBuilder WithPackageContentDirectory(string packageContentDirectory)
    {
        WithOption(PACKAGE_CONTENT_DIR_OPTION, packageContentDirectory);
        return this;
    }
    public PackageBuildArgBuilder WithPackageName(string packageName)
    {
        WithOption(PACKAGE_NAME_OPTION, packageName);
        return this;
    }

    public PackageBuildArgBuilder WithPackageVersion(string packageVersion)
    {
        WithOption(PACKAGE_VERSION_OPTION, packageVersion);
        return this;
    }

    public PackageBuildArgBuilder WithPackageInstalationDir(string packageInstalationDir)
    {
        WithOption(PACKAGE_INSTALATION_DIR_OPTION, packageInstalationDir);
        return this;
    }

    public PackageBuildArgBuilder WithPackageDestinationDir(string packageInstalationDir)
    {
        WithOption(PACKAGE_DESTINATION_DIR_OPTION, packageInstalationDir);
        return this;
    }
}
