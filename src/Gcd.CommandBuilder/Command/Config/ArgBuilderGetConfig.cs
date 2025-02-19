using Gcd.Commands.Config;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Config;

public class ArgBuilderGetConfig : ArgBuilder
{
    public ArgBuilderGetConfig(Arguments arguments) : base(arguments)
    {
        WithArg("get");
    }

    public ArgBuilderGetConfig WithNipkgInstallerUri()
    {
        WithFlag(NipkgInstallerUriOption.NAME);
        return this;
    }


    public ArgBuilderGetConfig WithNipkgCmdPath()
    {
        WithFlag(NipkgCmdPathOption.NAME);
        return this;
    }
}
