using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgAddPackageToAzFeed;
using Gcd.Commands.NipkgPackageBuilderInit;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.Commands.NipkgPackageBuilserSetVersion;

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
                nipkg.UseInstallNipkg(serviceProvider);
                nipkg.UseDownloadNipkgCmd(serviceProvider);
                nipkg.UseTemplatedCmd(serviceProvider);
                nipkg.UsePackageCmd(serviceProvider);
                nipkg.UseNipkgAddPackageToAzFeedCmd(serviceProvider);
                nipkg.UseNipkgPullFeedMetaCmd(serviceProvider);
                nipkg.UseNipkgPushAzBlobFeedMetaCmd(serviceProvider);
            });
            return app;
        }

        public static CommandLineApplication UseInstallNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            app.Command("install-nipkg", template =>
            {
                template.OnExecute(async () =>
                {
                    var request = new InstallNinpkgRequest();
                    var response = await mediator.Send(request);
                    console.WriteLine(response.result);
                });

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
                template.UsePackageBuilderSetVersionCmd(serviceProvider);
            });

            return app;
        }


        public static CommandLineApplication UsePackageBuilderSetVersionCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            app.Command("set-version", create =>
            {
                create.Description = "Create package template";
                var packagePath = create.Option("--package-path", "Directory where package will be created", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageVersion = create.Option("--package-version", "Package version.", CommandOptionType.SingleValue)
                    .IsRequired();


                create.OnExecute(async () =>
                {
                    var request = new PackageBuilderSetVersionRequest(packagePath.Value(), packageVersion.Value());
                    var response = await mediator.Send(request);
                    console.WriteLine(response.result);
                });
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
