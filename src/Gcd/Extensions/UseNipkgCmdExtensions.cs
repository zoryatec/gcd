using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using Gcd.Commands.NipkgAddPackageToAzFeed;
using Gcd.Commands.NipkgInstallNipkg;
using Gcd.Commands.NipkgPushAzBlobFeedMeta;
using Gcd.Commands.Nipkg.Builder.Init;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Commands.Nipkg.Build;


namespace Gcd.Extensions
{
    public static class UseNipkgCmdExtensions
    {
        public static CommandLineApplication UseNipkgCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();

            app.Command("nipkg", nipkg =>
            {
                nipkg.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    nipkg.ShowHelp();
                    return 1;
                });
                nipkg.UseInstallNipkgCmd(serviceProvider);
                nipkg.UseDownloadNipkgCmd(serviceProvider);
                nipkg.UseTemplatedCmd(serviceProvider);
                nipkg.UsePackageCmd(serviceProvider);
                nipkg.UseNipkgAddPackageToAzFeedCmd(serviceProvider);
                nipkg.UseNipkgPullFeedMetaCmd(serviceProvider);
                nipkg.UseNipkgPushAzBlobFeedMetaCmd(serviceProvider);
            });
            return app;
        }

        public static CommandLineApplication UseTemplatedCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            app.Command("package-builder", template =>
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



        public static CommandLineApplication UsePackageCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            app.Command("package", template =>
            {
                template.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    template.ShowHelp();
                    return 1;
                });
                template.UseNipkgPackageBuildCmd(serviceProvider);
            });

            return app;
        }


        
    }
}
