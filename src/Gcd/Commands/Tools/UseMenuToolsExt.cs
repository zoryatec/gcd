﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Tools;

public static class UseMenuToolsExt
{
    public static readonly string NAME = "tools";
    public static readonly bool   SHOW_IN_HELP = true;
    private static string DESCRIPTION = "Commands for tasks that does not fit in any other category.";
    public static CommandLineApplication UseMenuTools(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseCmdAddToUserPath(serviceProvider);
            cmd.UseCmdAddToSystemPath(serviceProvider);
            cmd.UseCmdAddToPath(serviceProvider);
            cmd.UseCmdBootstrap(serviceProvider);
            cmd.UseCmdInstallNipkg(serviceProvider);
            cmd.UseCmdDownloadNipkg(serviceProvider);
            cmd.UseCmdDownloadArchive(serviceProvider);
            cmd.UseCmdSetupGit(serviceProvider);
            cmd.UseCmdSetupRclone(serviceProvider);
            cmd.UseCmdSetIniFileParameter(serviceProvider);
            cmd.UseCmdSetupSystemForCi(serviceProvider);
        });
        return app;
    }



}