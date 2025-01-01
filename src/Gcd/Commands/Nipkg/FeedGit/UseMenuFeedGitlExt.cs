using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedGit;

public static class UseMenuFeedGitlExt
{
    public static CommandLineApplication UseMenuFeedGit(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("feed-git", cmd =>
        {

            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseAddLocalPackageToGitCmd(serviceProvider);
            cmd.UseAddHttpPackageCmdToGit(serviceProvider);
        });

        return app;
    }
}

