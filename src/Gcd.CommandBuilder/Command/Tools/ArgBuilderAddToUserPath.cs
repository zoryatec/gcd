using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Tools;

public class ArgBuilderAddToUserPath : ArgBuilder
{
    public ArgBuilderAddToUserPath(Arguments arguments) : base(arguments)
    {
        WithArg("add-to-user-path");
    }

    public ArgBuilderAddToUserPath WithPath(string pathToAdd)
    {
        WithArg(pathToAdd);
        return this;
    }
}
