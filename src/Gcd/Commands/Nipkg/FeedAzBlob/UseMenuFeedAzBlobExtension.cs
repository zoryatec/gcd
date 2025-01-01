using Gcd.Commands.Nipkg.Feed.AddPackageAz;
using Gcd.Commands.Nipkg.Feed.PullMetaDataAz;
using Gcd.Commands.Nipkg.Feed.PushMetaDataAz;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.Nipkg.FeedAzBlob;

public static class UseMenuFeedAzBlobExtension
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

            cmd.UseNipkgAddPackageToAzFeedCmd(serviceProvider);
            cmd.UsePullFeedMetaCmd(serviceProvider);
            cmd.UseNipkgPushAzBlobFeedMetaCmd(serviceProvider);

        });

        return app;
    }
}

