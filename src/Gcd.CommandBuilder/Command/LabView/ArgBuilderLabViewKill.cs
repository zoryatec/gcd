using Gcd.Commands.LabView;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.LabView;

public class ArgBuilderLabViewKill : ArgBuilder
{
    public ArgBuilderLabViewKill(Arguments arguments) : base(arguments)
    {
        _arguments.Add(UseCmdKilExt.NAME);
    }
}