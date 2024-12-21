using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Gcd.Extensions;
using Gcd.Commands.Nipkg.Builder.AddContent;
using Gcd.Commands.Nipkg.Builder.Init;
using Gcd.Nipkg.Instructions.Model;


namespace Gcd.Commands.Nipkg.Builder.AddInstruction;

public static class UseAddInstructionCmdExtensions
{
    public static CommandLineApplication UseAddCustomExecuteCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command("add-custom-execute", command =>
        {
            command.Description = "COMMAND_DESCRIPTION";
            var builderRootDirArg = new PackageBuilderRootDirArgument();
            var rootOpt = new CustomExecuteRootOption();
            var argsOpt = new CustomExecuteArgumentsOption();
            var exeNameOpt = new CustomExecuteExeNameOption();
            var stepOpt = new CustomExecuteStepOption();
            var scheduleOpt = new CustomExecuteScheduleOption();
            command.AddArgument(builderRootDirArg.IsRequired());
            command.AddOptions(
                rootOpt.IsRequired(),
                argsOpt.IsRequired(),
                exeNameOpt.IsRequired(),
                stepOpt.IsRequired(),
                scheduleOpt.IsRequired()
                );



            command.OnExecuteAsync(async cancelationToken =>
            {
                return 0;
                var root = rootOpt.Map();
                var args = argsOpt.Map();
                var exeName = exeNameOpt.Map();
                var step = stepOpt.Map();
                var schedule = scheduleOpt.Map();
                var value = builderRootDirArg.Value;
                var builderRootDir = builderRootDirArg.Map();


                return await Result
                    .Combine(builderRootDir, exeName, args, step, schedule)
                    .Map(() => new FilePackageCustomeExecute(root.Value, exeName.Value, args.Value, step.Value, schedule.Value))
                    .Bind((custExe) => mediator.AddInstructionAsync(builderRootDir.Value, custExe, cancelationToken))
                    .Tap(() => console.Write("SUCESS_MESSAGE"))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}



public class CustomExecuteRootOption : CommandOption
{
    public static readonly string NAME = "--root";
    public CustomExecuteRootOption() : base(NAME, CommandOptionType.SingleValue)
    {
        this.Description = "Description";
    }
    public Result<CustomExecuteRoot> Map() =>
        CustomExecuteRoot.Of(this.Value());
}

public class CustomExecuteArgumentsOption : CommandOption
{
    public static readonly string NAME = "--arguments";
    public CustomExecuteArgumentsOption() : base(NAME, CommandOptionType.SingleValue)
    {
        this.Description = "Description";
    }
    public Result<CustomExecuteArguments> Map() =>
    CustomExecuteArguments.Of(this.Value());
}


public class CustomExecuteExeNameOption : CommandOption
{
    public static readonly string NAME = "--exe-name";
    public CustomExecuteExeNameOption() : base(NAME, CommandOptionType.SingleValue)
    {
        this.Description = "Description";
    }
    public Result<CustomExecuteExeName> Map() =>
        CustomExecuteExeName.Of(this.Value());
}

public class CustomExecuteStepOption : CommandOption
{
    public static readonly string NAME = "--step";
    public CustomExecuteStepOption() : base(NAME, CommandOptionType.SingleValue)
    {
        this.Description = "Description";
    }
    public Result<CustomExecuteStep> Map() =>
        CustomExecuteStep.Of(this.Value());
}

public class CustomExecuteScheduleOption : CommandOption
{
    public static readonly string NAME = "--schedule";
    public CustomExecuteScheduleOption() : base(NAME, CommandOptionType.SingleValue)
    {
        this.Description = "Description";
    }
    public Result<CustomExecuteSchedule> Map() =>
        CustomExecuteSchedule.Of(this.Value());
}