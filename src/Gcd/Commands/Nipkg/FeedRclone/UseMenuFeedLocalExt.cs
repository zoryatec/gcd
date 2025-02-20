using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedRclone;

public static class UseMenuFeedRcloneExt
{
    public static string NAME = "feed-rclone";
    public static string DESCRIPTION = "Commands for managing nipkg feed proxied through Rclone remotes.";
    public static CommandLineApplication UseMenuFeedRclone(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                var helpText =
@"
Commands for managing nipkg feeds access through Rclone.
";              
                console.WriteLine(helpText);
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseCmdAddLocalPackage(serviceProvider);
            cmd.UseCmdAddDirectory(serviceProvider);
        });

        return app;
    }
}

