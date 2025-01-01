using Gcd.Commands.Nipkg.FeedLocal.AddPackageLocal;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedLocal;

public static class UseMenuFeedLocalExtension
{
    public static CommandLineApplication UseFeedLocal(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("feed-local", cmd =>
        {

            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseAddLocalPackageCmd(serviceProvider);
            cmd.UseAddHttpPackageCmd(serviceProvider);

        });

        return app;
    }
}

