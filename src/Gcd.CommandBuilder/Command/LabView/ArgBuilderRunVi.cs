using Gcd.Commands.LabView;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.LabView;

public class ArgBuilderRunVi : ArgBuilderLabView<ArgBuilderRunVi>
{
    
    public ArgBuilderRunVi(Arguments arguments) : base(arguments)
    {
        WithArg(UseCmdRunViExt.NAME);
    }
    
    public ArgBuilderRunVi WithViPathOption(string value)
    {
        WithOption(LabViewViPathOption.NAME, value);
        return this;
    }
    
    public ArgBuilderRunVi WithViArgument(string value)
    {
        WithArg(value);
        return this;
    }
}