using Gcd.Commands.LabView;
using Gcd.Commands.Project.BuildSpec;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.LabView;

public class ArgBuilderSetVersion : ArgBuilderLabView<ArgBuilderSetVersion>
{
    
    public ArgBuilderSetVersion(Arguments arguments) : base(arguments)
    {
        WithArg(UseCmdSetVersionExt.NAME);
    }
    
    public ArgBuilderSetVersion WithProjectPathOption(string value)
    {
        WithOption(LabViewProjectPathOption.NAME, value);
        return this;
    }
    
    public ArgBuilderSetVersion WithBuildSpecNameOption(string value)
    {
        WithOption(LabViewBuildSpecNameOption.NAME, value);
        return this;
    }
    
    public ArgBuilderSetVersion WithBuildSpecTypeOption(string value)
    {
        WithOption(LabViewBuildSpecTypeOption.NAME, value);
        return this;
    }
    
    public ArgBuilderSetVersion WithBuildSpecTargetOption(string value)
    {
        WithOption(LabViewBuildSpecTargetOption.NAME, value);
        return this;
    }
    
    public ArgBuilderSetVersion WithBuildSpecVersionOption(string value)
    {
        WithOption(LabViewBuildSpecVersionOption.NAME, value);
        return this;
    }
    
    public ArgBuilderSetVersion WithBuildSpecOutputDir(string value)
    {
        WithOption(LabViewBuildSpecOutputDirOption.NAME, value);
        return this;
    }
}