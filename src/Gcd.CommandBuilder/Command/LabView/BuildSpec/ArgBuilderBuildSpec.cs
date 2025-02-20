using Gcd.Commands.LabView;
using Gcd.Commands.Project;
using Gcd.Commands.Project.BuildSpec;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.LabView;

public class ArgBuilderBuildSpec : ArgBuilderLabView<ArgBuilderBuildSpec>
{
    
    public ArgBuilderBuildSpec(Arguments arguments) : base(arguments)
    {
        WithArg(UseCmdBuildExt.NAME);
    }
    
    public ArgBuilderBuildSpec WithProjectPathOption(string value)
    {
        WithOption(LabViewProjectPathOption.NAME, value);
        return this;
    }
    
    public ArgBuilderBuildSpec WithBuildSpecNameOption(string value)
    {
        WithOption(LabViewBuildSpecNameOption.NAME, value);
        return this;
    }
    
    public ArgBuilderBuildSpec WithBuildSpecTargetOption(string value)
    {
        WithOption(LabViewBuildSpecTargetOption.NAME, value);
        return this;
    }
    
    public ArgBuilderBuildSpec WithBuildSpecVersionOption(string value)
    {
        WithOption(LabViewBuildSpecVersionOption.NAME, value);
        return this;
    }
    
    public ArgBuilderBuildSpec WithBuildSpecOutputDir(string value)
    {
        WithOption(LabViewBuildSpecOutputDirOption.NAME, value);
        return this;
    }
}