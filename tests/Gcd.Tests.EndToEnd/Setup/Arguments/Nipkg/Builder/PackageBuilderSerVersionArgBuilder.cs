using Gcd.Commands.Nipkg.Builder;
using Gcd.Commands.Nipkg;
namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class PackageBuilderSerVersionArgBuilder : ArgumentsBuilder
{
    public PackageBuilderSerVersionArgBuilder()
    {
        WithArg(UseMenuNipkgExtension.NAME);
        WithArg(UseMenuBuilderExt.NAME);
        WithArg(UseCmdSetPropertyExt.NAME);
    }

    public PackageBuilderSerVersionArgBuilder WithPackageBuilderDirectory(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }
    public PackageBuilderSerVersionArgBuilder WithVersion(string value)
    {
        WithOption(PackageVersionOption.NAME, value);
        return this;
    }

    public PackageBuilderSerVersionArgBuilder WithHomePage(string value)
    {
        WithOption(PackageHomePageOption.NAME, value);
        return this;
    }

    public PackageBuilderSerVersionArgBuilder WithMaintainer(string value)
    {
        WithOption(PackageMaintainerOption.NAME, value);
        return this;
    }
}
