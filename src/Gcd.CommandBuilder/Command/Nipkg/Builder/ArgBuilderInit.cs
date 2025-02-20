using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.Builder;

public class ArgBuilderInit : ArgBuilder
{
    public ArgBuilderInit(Arguments arguments) : base(arguments)
    {
        _arguments.Add(UseCmdInitExt.NAME);
    }

    public ArgBuilderInit WithPackageBuilderDirectory(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }
    public ArgBuilderInit WithPackageName(string value)
    {
        WithOption(PackageNameOption.NAME, value);
        return this;
    }

    public ArgBuilderInit WithPackageVersion(string value)
    {
        WithOption(PackageVersionOption.NAME, value);
        return this;
    }
}
