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
using Gcd.Commands.Nipkg.Builder.AddInstruction;


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
                cmd.UseFeedLocal(serviceProvider);
            });
            return app;
        }

        public static CommandLineApplication UseBuilderCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            app.Command("builder", cmd =>
            {
                cmd.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    cmd.ShowHelp();
                    return 1;
                });
                cmd.UseNipkgPackageBuilderInitmd(serviceProvider);
                cmd.UseNipkgPackageBuilderSetPropertyCmd(serviceProvider);
                cmd.UseAddContentCmd(serviceProvider);
                cmd.UseFilePackageInstructionsCmd(serviceProvider);
                //cmd.UseMsiPackageInstructionsCmd(serviceProvider);
                cmd.UsePackCmd(serviceProvider);
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

        public static CommandLineApplication UseFilePackageInstructionsCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            app.Command("instructions-file-pkg", cmd =>
            {

                cmd.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    cmd.ShowHelp();
                    return 1;
                });

                cmd.UseAddCustomExecuteCmd(serviceProvider);

            });

            return app;
        }

        public static CommandLineApplication UseMsiPackageInstructionsCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            app.Command("instructions-msi-pkg", cmd =>
            {

                cmd.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    cmd.ShowHelp();
                    return 1;
                });

            });

            return app;
        }

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

            });

            return app;
        }
    }
}
