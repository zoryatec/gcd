﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.Nipkg.FeedAzBlob;

public static class UseMenuFeedAzBlobExt
{
    public static CommandLineApplication UseFeedAzBlob(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("feed", cmd =>
        {

            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseCmdAddLocalPackage(serviceProvider);
            cmd.UseCmdPullMetaData(serviceProvider);
            cmd.UseCmdPushMetaData(serviceProvider);

        });

        return app;
    }
}
