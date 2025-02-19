using Gcd.Commands.LabView;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.LabView;

public class ArgBuilderBuildProject : ArgBuilderLabView<ArgBuilderBuildProject>
{
    
    public ArgBuilderBuildProject(Arguments arguments) : base(arguments)
    {
        WithArg(UseBuildProjectExt.NAME);
    }
    
    public ArgBuilderBuildProject WithProjectVersionOption(string value)
    {
        WithOption(LabViewProjectVersionOption.NAME, value);
        return this;
    }
    
    public ArgBuilderBuildProject WithProjectOutputDirOption(string value)
    {
        WithOption(ProjectOutputDirOption.NAME, value);
        return this;
    }
    public ArgBuilderBuildProject WithProjectPathOption(string value)
    {
        WithOption(LabViewProjectPathOption.NAME, value);
        return this;
    }
}