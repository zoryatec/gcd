﻿using CSharpFunctionalExtensions;
using Gcd.Handlers.Setup;
using Gcd.Handlers.Tools;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Tools;

public static class UseCmdSetupGitExt
{
    public static readonly string NAME = "setup-git";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = false;
    public static readonly string DESCRIPTION = "setup rclone";
    
    public static CommandLineApplication UseCmdSetupGit(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
    
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                return await mediator.Send(new SetupGitRequest(), cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

