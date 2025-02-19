using Gcd.Commands.LabView;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.LabView;

public abstract class ArgBuilderLabView<TSelf>(Arguments arguments) : ArgBuilder(arguments)
    where TSelf : ArgBuilderLabView<TSelf>
{
    public TSelf WithLabViewPathOption(string value)
    {
        WithOption(LabViewPathOption.NAME, value);
        return (TSelf) this;
    }
    
    public TSelf WithLabViewPortOption(string value)
    {
        WithOption(LabViewPortOption.NAME, value);
        return (TSelf) this;
    }
}