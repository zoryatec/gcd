using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedLocal;

public static class UseMenuFeedLocalExt
{
    public static string NAME = "feed-local";
    public static string DESCRIPTION = "Commands for managing nipkg feed hosted on local file system.";
    public static CommandLineApplication UseMenuFeedLocal(this CommandLineApplication app, IServiceProvider serviceProvider)
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
            cmd.UseCmdAddHttpPackage(serviceProvider);
            cmd.UseCmdAddLocalDirectory(serviceProvider);

        });

        return app;
    }
}

