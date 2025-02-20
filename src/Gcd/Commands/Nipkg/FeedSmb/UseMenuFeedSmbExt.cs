using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedSmb;

public static class UseMenuFeedSmbExt
{
    public static string NAME = "feed-smb";
    public static string DESCRIPTION = "Commands for managing nipkg feed hosted on smb file share. Only user and password authentication is supported.";
    public static CommandLineApplication UseMenuFeedSmb(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseCmdAddLocalPackage(serviceProvider);
            cmd.UseCmdPushMeta(serviceProvider);
            cmd.UseCmdPullMeta(serviceProvider);
            cmd.UseCmdAddDirectory(serviceProvider);
        });

        return app;
    }
}

