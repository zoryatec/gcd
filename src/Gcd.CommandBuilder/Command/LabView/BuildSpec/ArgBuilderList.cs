using Gcd.Commands.LabView;
using Gcd.Commands.Project.BuildSpec;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.LabView;

public class ArgBuilderList : ArgBuilderLabView<ArgBuilderList>
{
    
    public ArgBuilderList(Arguments arguments) : base(arguments)
    {
        WithArg(UseCmdListExt.NAME);
    }
    
    public ArgBuilderList WithProjectPathOption(string value)
    {
        WithOption(LabViewProjectPathOption.NAME, value);
        return this;
    }
    
    public ArgBuilderList WithBuildSpecNameOption(string value)
    {
        WithOption(LabViewBuildSpecNameOption.NAME, value);
        return this;
    }
    
    public ArgBuilderList WithBuildSpecTypeOption(string value)
    {
        WithOption(LabViewBuildSpecTypeOption.NAME, value);
        return this;
    }
    
    public ArgBuilderList WithBuildSpecTargetOption(string value)
    {
        WithOption(LabViewBuildSpecTargetOption.NAME, value);
        return this;
    }
    
    public ArgBuilderList WithBuildSpecVersionOption(string value)
    {
        WithOption(LabViewBuildSpecVersionOption.NAME, value);
        return this;
    }
    
    public ArgBuilderList WithBuildSpecOutputDir(string value)
    {
        WithOption(LabViewBuildSpecOutputDirOption.NAME, value);
        return this;
    }
}