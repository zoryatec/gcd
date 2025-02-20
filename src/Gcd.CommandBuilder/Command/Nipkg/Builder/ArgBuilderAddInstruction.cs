using Gcd.Commands.Nipkg;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Tests.EndToEnd.Setup.Arguments;

namespace Gcd.CommandBuilder.Command.Nipkg.Builder;

public class ArgBuilderAddInstruction : ArgBuilder
{
    public ArgBuilderAddInstruction(Arguments arguments) : base(arguments)
    {
        _arguments.Add("instructions-file-pkg","add-custom-execute");
    }
    public ArgBuilderAddInstruction WithPackageBuilderRootDir(string value)
    {
        WithOption(BuilderRootDirOption.NAME, value);
        return this;
    }

    public ArgBuilderAddInstruction WithExeName(string value)
    {
        WithOption(CustomExecuteExeNameOption.NAME, value);
        return this;
    }


    public ArgBuilderAddInstruction WithArugments(string value)
    {
        WithOption(CustomExecuteArgumentsOption.NAME, value);
        return this;
    }

    public ArgBuilderAddInstruction WithRoot(string value)
    {
        WithOption(CustomExecuteRootOption.NAME, value);
        return this;
    }
    public ArgBuilderAddInstruction WithStep(string value)
    {
        WithOption(CustomExecuteStepOption.NAME, value);
        return this;
    }
    public ArgBuilderAddInstruction WithSchedule(string value)
    {
        WithOption(CustomExecuteScheduleOption.NAME, value);
        return this;
    }
}
