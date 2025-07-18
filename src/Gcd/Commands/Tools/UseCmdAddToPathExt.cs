﻿using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Handlers.Tools;

namespace Gcd.Commands.Tools;

public static class UseCmdAddToPathExt
{
    private static bool SHOW_IN_HELP = true;
    private static string DESCRIPTION = "Command to add provided path to System PATH variable. It falls back into User Path.";
    public static CommandLineApplication UseCmdAddToPath(this CommandLineApplication app,
       IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-to-path", cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.ShowInHelpText = SHOW_IN_HELP;

            var pathToAdd = cmd.Argument("path", "Path to be added to user path enviromental variable").IsRequired();

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var maybePath = Maybe.From(pathToAdd.Value);
                if (maybePath.HasValue)
                {
                    var response = await mediator.AddToSystemPath(maybePath.Value, cancelationToken);
                    
                    if (response.IsFailure)
                    {
                         response = await mediator.AddToUserPath(maybePath.Value, cancelationToken);
                    }

                    return response
                        .Tap(() => console.Write("Path added sucessfully"))
                        .TapError(error => console.Error.Write(error))
                        .Finally(x => x.IsFailure ? 1 : 0);

                }
                console.Error.Write("Path not given");
                return 1;

            });
        });
        return app;
    }
}