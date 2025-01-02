using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedSmb;

public static class UseMenuFeedSmbExt
{
    public static CommandLineApplication UseMenuFeedSmb(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("feed-smb", cmd =>
        {

            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseCmdAddLocalPackage(serviceProvider);
            cmd.UseCmdPushMeta(serviceProvider);
        });

        return app;
    }
}

