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
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

            FeedGit.UUseAddLocalPackageCmdExtensions.UseCmdAddLocalPackage(cmd, serviceProvider);
            FeedGit.UseCmdAddHttpPackageExt.UseCmdAddHttpPackage(cmd, serviceProvider);
            FeedGit.UseCmdPullFeedMetaExt.UseCmdPullFeedMeta(cmd, serviceProvider);
            FeedGit.UseCmdPushFeedMetaExt.UseCmdPushFeedMeta(cmd, serviceProvider);
        });

        return app;
    }
}

