using static Gcd.Contract.Nipkg.PackageBuilderSetVersion;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class PackageBuilderSerVersionArgBuilder : ArgumentsBuilder
{
    public PackageBuilderSerVersionArgBuilder()
    {
        WithArg("nipkg");
        WithArg("package-builder");
        WithArg(COMMAND);
    }

    public PackageBuilderSerVersionArgBuilder WithPackageBuilderDirectory(string value)
    {
        WithOption(PACKAGE_PATH_OPTION, value);
        return this;
    }
    public PackageBuilderSerVersionArgBuilder WithVersion(string value)
    {
        WithOption(PACKAGE_VERSION_OPTION, value);
        return this;
    }
}
