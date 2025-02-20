using Gcd.Commands.Vipm;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Vipm;

public class ArgBuilderVipmKill : ArgBuilder
{
    public ArgBuilderVipmKill(Arguments arguments) : base(arguments)
    {
        _arguments.Add(UseCmdKilExt.NAME);
    }
}