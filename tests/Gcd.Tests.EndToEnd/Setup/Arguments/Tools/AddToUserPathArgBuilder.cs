using Gcd.Commands.Tools;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg;

public class AddToUserPathArgBuilder : ArgumentsBuilder
{
    public AddToUserPathArgBuilder()
    {
        WithArg(UseMenuToolsExt.NAME);
        WithArg("add-to-user-path");
    }

    public AddToUserPathArgBuilder WithPath(string pathToAdd)
    {
        WithArg(pathToAdd);
        return this;
    }
}
