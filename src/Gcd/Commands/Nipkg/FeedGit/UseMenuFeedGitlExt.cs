using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedGit;

public static class UseMenuFeedGitlExt
{
    public static string NAME = "feed-git";
    public static string DESCRIPTION = "Commands for managing nipkg feed hosted on git repository. This feature is experimental.";
    public static CommandLineApplication UseMenuFeedGit(this CommandLineApplication app, IServiceProvider serviceProvider)
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
            cmd.UseCmdPullFeedMeta(serviceProvider);
            cmd.UseCmdPushFeedMeta(serviceProvider);
        });

        return app;
    }
}

