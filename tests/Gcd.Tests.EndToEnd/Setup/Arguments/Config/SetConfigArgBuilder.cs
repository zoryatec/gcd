using Gcd.Commands.Config;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;

public class SetConfigArgBuilder : ArgumentsBuilder
{
    public SetConfigArgBuilder()
    {
        WithArg("config");
        WithArg("set");
    }

    public SetConfigArgBuilder WithNipkgInstallerUri(string value)
    {
        WithOption(NipkgInstallerUriOption.NAME, value);
        return this;
    }


    public SetConfigArgBuilder WithNipkgCmdPath(string value)
    {
        WithOption(NipkgCmdPathOption.NAME, value);
        return this;
    }
}
