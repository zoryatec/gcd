using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedGitHub;

public static class UseMenuFeedGitHublExt
{
    public static string NAME = "feed-github";
    public static string DESCRIPTION = "Commands for managing nipkg feed hosted on github repository. This feature is experimental.";
    public static CommandLineApplication UseMenuFeedGitHub(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command(NAME, menu =>
        {
            menu.Description = DESCRIPTION;
            menu.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                menu.ShowHelp();
                return 1;
            });

            menu.UseCmdAddLocalPackage(serviceProvider);
            menu.UseCmdAddHttpPackage(serviceProvider);
            menu.UseCmdPullFeedMeta(serviceProvider);
            menu.UseCmdPushFeedMeta(serviceProvider);
        });

        return app;
    }
}

