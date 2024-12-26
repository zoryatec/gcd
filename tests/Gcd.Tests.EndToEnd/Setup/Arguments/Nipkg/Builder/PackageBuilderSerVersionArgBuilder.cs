using static Gcd.Contract.Nipkg.PackageBuilderSetProperty;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class PackageBuilderSerVersionArgBuilder : ArgumentsBuilder
{
    public PackageBuilderSerVersionArgBuilder()
    {
        WithArg("nipkg");
        WithArg("builder");
        WithArg(COMMAND);
    }

    public PackageBuilderSerVersionArgBuilder WithPackageBuilderDirectory(string value)
    {
        WithOption(PACKAGE_BUILDER_DIR_OPTION, value);
        return this;
    }
    public PackageBuilderSerVersionArgBuilder WithVersion(string value)
    {
        WithOption(PACKAGE_VERSION_OPTION, value);
        return this;
    }

    public PackageBuilderSerVersionArgBuilder WithHomePage(string value)
    {
        WithOption(PACKAGE_HOME_PAGE_OPTION, value);
        return this;
    }

    public PackageBuilderSerVersionArgBuilder WithMaintainer(string value)
    {
        WithOption(PACKAGE_MAINTAINER_OPTION, value);
        return this;
    }
}
