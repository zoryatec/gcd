using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Gcd.Commands.Nipkg.Builder.Init;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Commands.Nipkg.Build;
using Gcd.Commands.Nipkg.Feed.PullMetaDataAz;
using Gcd.Commands.Nipkg.Feed.PushMetaDataAz;
using Gcd.Commands.Nipkg.Feed.AddPackageAz;
using Gcd.Commands.Tools.InstallNipkg;
using Gcd.Commands.Tools.DownloadNipkg;


namespace Gcd.Menu
{
    public static class UseNipkgCmdExtensions
    {
        public static CommandLineApplication UseNipkgCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();

            app.Command("nipkg", cmd =>
            {
                cmd.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    cmd.ShowHelp();
                    return 1;
                });
                cmd.UseBuilderCmd(serviceProvider);
                cmd.UseFeedCmd(serviceProvider);
                cmd.UseNipkgPackageBuildCmd(serviceProvider);
            });
            return app;
        }

        public static CommandLineApplication UseBuilderCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            app.Command("builder", template =>
            {
                template.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    template.ShowHelp();
                    return 1;
                });
                template.UseNipkgPackageBuilderInitmd(serviceProvider);
                template.UseNipkgPackageBuilderSetPropertyCmd(serviceProvider);
                template.UseAddContentCmd(serviceProvider);
            });

            return app;
        }



        public static CommandLineApplication UseFeedCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
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
                cmd.UseNipkgPullFeedMetaCmd(serviceProvider);
                cmd.UseNipkgPushAzBlobFeedMetaCmd(serviceProvider);

            });

            return app;
        }



    }
}
