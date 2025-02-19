using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.Builder;

public class ArgBuilderSetVersion : ArgBuilder
{
    public ArgBuilderSetVersion(Arguments arguments) : base(arguments)
    {
        arguments.Add(UseCmdSetPropertyExt.NAME);
    }

    public ArgBuilderSetVersion WithPackageBuilderDirectory(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }
    public ArgBuilderSetVersion WithVersion(string value)
    {
        WithOption(PackageVersionOption.NAME, value);
        return this;
    }

    public ArgBuilderSetVersion WithHomePage(string value)
    {
        WithOption(PackageHomePageOption.NAME, value);
        return this;
    }

    public ArgBuilderSetVersion WithMaintainer(string value)
    {
        WithOption(PackageMaintainerOption.NAME, value);
        return this;
    }
}
