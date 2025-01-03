using Gcd.Commands.Config;
using Gcd.Commands.Nipkg.Builder.Init;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;

public class GetConfigArgBuilder : ArgumentsBuilder
{
    public GetConfigArgBuilder()
    {
        WithArg("config");
        WithArg("get");
    }

    public GetConfigArgBuilder WithNipkgInstallerUri()
    {
        WithFlag(NipkgInstallerUriOption.NAME);
        return this;
    }


    public GetConfigArgBuilder WithNipkgCmdPath()
    {
        WithFlag(NipkgCmdPathOption.NAME);
        return this;
    }
}
