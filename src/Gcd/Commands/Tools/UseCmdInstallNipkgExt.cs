﻿
using CSharpFunctionalExtensions;
using Gcd.Handlers.Tools;
using Gcd.Model.Config;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.InstallNipkg;


namespace Gcd.Commands.Tools;

public static class UseCmdInstallNipkgExt
{
    public static CommandLineApplication UseCmdInstallNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(COMMAND, subCmd =>
        {
            subCmd.Description = COMMAND_DESCRIPTION;

            subCmd.OnExecuteAsync(async cancelationToken =>
            {
                var cmdPath = NipkgCmdPath.None;
                return await mediator.InstallNipkgInstallerAsync(cmdPath, cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}
