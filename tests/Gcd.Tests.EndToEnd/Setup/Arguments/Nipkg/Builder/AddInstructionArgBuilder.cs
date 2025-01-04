using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Handlers.Nipkg.Builder;

namespace Gcd.Tests.EndToEnd.Arguments.Nipkg.Builder;

public class AddInstructionArgBuilder : ArgumentsBuilder
{
    public AddInstructionArgBuilder()
    {
        WithArg("nipkg");
        WithArg("builder");
        WithArg("instructions-file-pkg");
        WithArg("add-custom-execute");
    }
    public AddInstructionArgBuilder WithPackageBuilderRootDir(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }

    public AddInstructionArgBuilder WithExeName(string value)
    {
        WithOption(CustomExecuteExeNameOption.NAME, value);
        return this;
    }


    public AddInstructionArgBuilder WithArugments(string value)
    {
        WithOption(CustomExecuteArgumentsOption.NAME, value);
        return this;
    }

    public AddInstructionArgBuilder WithRoot(string value)
    {
        WithOption(CustomExecuteRootOption.NAME, value);
        return this;
    }
    public AddInstructionArgBuilder WithStep(string value)
    {
        WithOption(CustomExecuteStepOption.NAME, value);
        return this;
    }
    public AddInstructionArgBuilder WithSchedule(string value)
    {
        WithOption(CustomExecuteScheduleOption.NAME, value);
        return this;
    }
}
