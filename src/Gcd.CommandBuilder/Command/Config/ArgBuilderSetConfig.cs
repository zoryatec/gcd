using Gcd.Commands.Config;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Config;

public class ArgBuilderSetConfig : ArgBuilder
{
    public ArgBuilderSetConfig(Arguments arguments) : base(arguments)
    {
        WithArg("set");
    }

    public ArgBuilderSetConfig WithNipkgInstallerUri(string value)
    {
        WithOption(NipkgInstallerUriOption.NAME, value);
        return this;
    }


    public ArgBuilderSetConfig WithNipkgCmdPath(string value)
    {
        WithOption(NipkgCmdPathOption.NAME, value);
        return this;
    }
}
