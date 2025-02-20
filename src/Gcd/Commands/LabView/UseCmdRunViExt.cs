using CSharpFunctionalExtensions;
using Gcd.Commands.Tools;
using Gcd.Extensions;
using Gcd.Handlers.LabView;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.LabView;

public static class UseCmdRunViExt
{
    public static readonly string NAME = "run-vi";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = true;
    private static string DESCRIPTION = "Command is a wrapper around LabVIEW CLI RunVI operation.";
    
    public static CommandLineApplication UseCmdRunVi(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.Description = DESCRIPTION;
            var viPathOption = new LabViewViPathOption();
            var lvPathOption = new LabViewPathOption();
            var lvPortOption = new LabViewPortOption();
            cmd.AddOptions(
                viPathOption.IsRequired(),
                lvPathOption.IsRequired(),
                lvPortOption.IsRequired()
                );

            var viArguments = cmd.Argument("[arguments]...", "Arguments passed to Vi");
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var viPath = viPathOption.Map();
                var lvPath = lvPathOption.Map();
                var lvPort = lvPortOption.Map();
                var args = viArguments.Values;
                
                var response = await mediator.RunViAsync(viPath.Value, args,LabViewCliCmdPath.None, lvPath.Value, lvPort.Value,  cancellationToken: cancelationToken);

                
                return response
                    .Tap((rsp) => console.Write(rsp))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

