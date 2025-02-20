using CSharpFunctionalExtensions;
using Gcd.Commands.Tools;
using Gcd.Extensions;
using Gcd.Handlers.LabView;
using Gcd.Handlers.Tools;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.LabView;

public static class UseCmdKilExt
{
    public static readonly string NAME = "kill";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = false;
    private static string DESCRIPTION = "Command kills LabVIEW process if running.";
    
    public static CommandLineApplication UseCmdKill(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
    
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                return await mediator.KillProcessAsync(ProcessName.LabView, cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

