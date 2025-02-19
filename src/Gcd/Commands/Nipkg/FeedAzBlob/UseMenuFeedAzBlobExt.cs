using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.Nipkg.FeedAzBlob;

public static class UseMenuFeedAzBlobExt
{
    public static string NAME = "feed-az-blob";
    public static string DESCRIPTION = "Commands for managing nipkg feed hosted on azure blob storage. Only SAS authentication is supported.";
    public static CommandLineApplication UseMenuFeedAzBlob(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.OnExecute(() =>
            {
                console.WriteLine("");
                cmd.ShowHelp();
                return 1;
            });

            cmd.UseCmdAddLocalPackage(serviceProvider);
            cmd.UseCmdPullMetaData(serviceProvider);
            cmd.UseCmdPushMetaData(serviceProvider);

        });

        return app;
    }
}

