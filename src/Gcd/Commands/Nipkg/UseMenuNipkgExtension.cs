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
using Gcd.Commands.Nipkg.FeedLocal.AddPackageLocal;
using Gcd.Commands.Nipkg.FeedGit;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Commands.Nipkg.FeedAzBlob;
using Gcd.Commands.Nipkg.FeedLocal;


namespace Gcd.Commands.Nipkg
{
    public static class UseMenuNipkgExtension
    {
        public static CommandLineApplication UseMenuNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
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
                cmd.UseMenuBuilder(serviceProvider);
                cmd.UseFeedAzBlob(serviceProvider);
                cmd.UseCmdBuild(serviceProvider);
                cmd.UseFeedLocal(serviceProvider);
                cmd.UseFeedGit(serviceProvider);
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



        public static CommandLineApplication UseFeedGit(this CommandLineApplication app, IServiceProvider serviceProvider)
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
}
