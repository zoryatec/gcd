using CSharpFunctionalExtensions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Gcd.Extensions;
using Gcd.Nipkg.Instructions.Model;
using Gcd.Handlers.Nipkg.Builder;


namespace Gcd.Commands.Nipkg.Builder;

public static class UseCmdAddInstructionExt
{
    public static readonly string NAME = "add-custom-execute";
    public static readonly string DESCRIPTION = "inadd-custom-executeit";
    public static readonly string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdAddInstruction(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.ShowInHelpText = false;
            
            var builderRootDirOpt = new BuilderRootDirOption();
            var rootOpt = new CustomExecuteRootOption();
            var argsOpt = new CustomExecuteArgumentsOption();
            var exeNameOpt = new CustomExecuteExeNameOption();
            var stepOpt = new CustomExecuteStepOption();
            var scheduleOpt = new CustomExecuteScheduleOption();
            cmd.AddOptions(
                builderRootDirOpt.IsRequired(),
                rootOpt.IsRequired(),
                argsOpt.IsRequired(),
                exeNameOpt.IsRequired(),
                stepOpt.IsRequired(),
                scheduleOpt.IsRequired()
                );



            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var root = rootOpt.Map();
                var args = argsOpt.Map();
                var exeName = exeNameOpt.Map();
                var step = stepOpt.Map();
                var schedule = scheduleOpt.Map();
                //var value = builderRootDirArg.Value;
                var builderRootDir = builderRootDirOpt.Map();


                return await Result
                    .Combine(builderRootDir, exeName, args, step, schedule)
                    .Map(() => new FilePackageCustomeExecute(root.Value, exeName.Value, args.Value, step.Value, schedule.Value))
                    .Bind((custExe) => mediator.AddInstructionAsync(builderRootDir.Value, custExe, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
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
        Description = "Description";
    }
    public Result<CustomExecuteRoot> Map() =>
        CustomExecuteRoot.Of(Value());
}

public class CustomExecuteArgumentsOption : CommandOption
{
    public static readonly string NAME = "--arguments";
    public CustomExecuteArgumentsOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<CustomExecuteArguments> Map() =>
    CustomExecuteArguments.Of(Value());
}


public class CustomExecuteExeNameOption : CommandOption
{
    public static readonly string NAME = "--exe-name";
    public CustomExecuteExeNameOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<CustomExecuteExeName> Map() =>
        CustomExecuteExeName.Of(Value());
}

public class CustomExecuteStepOption : CommandOption
{
    public static readonly string NAME = "--step";
    public CustomExecuteStepOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<CustomExecuteStep> Map() =>
        CustomExecuteStep.Of(Value());
}

public class CustomExecuteScheduleOption : CommandOption
{
    public static readonly string NAME = "--schedule";
    public CustomExecuteScheduleOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<CustomExecuteSchedule> Map() =>
        CustomExecuteSchedule.Of(Value());
}