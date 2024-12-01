using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                nipkg.UseDownloadlNipkg(serviceProvider);
                nipkg.UseTemplatedCmd(serviceProvider);
                nipkg.UsePackageCmd(serviceProvider);
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

        public static CommandLineApplication UseDownloadlNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            app.Command("download-nipkg", subCmd =>
            {
                subCmd.Description = "Create package template";
                var downloadPath = subCmd.Option("--download-path", "File path must end with exe", CommandOptionType.SingleValue);
                subCmd.OnExecute(async () =>
                {
                    var request = new DownloadNipkgRequest(downloadPath.Value());
                    var response = await mediator.Send(request);
                    console.WriteLine(response.Result);
                });

            });

            return app;
        }

        public static CommandLineApplication UseTemplatedCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            app.Command("template", template =>
            {
                template.OnExecute(() =>
                {
                    console.WriteLine("Specify a subcommand");
                    template.ShowHelp();
                    return 1;
                });
                template.UseTemplatedCreateCmd(serviceProvider);
            });

            return app;
        }

        public static CommandLineApplication UseTemplatedCreateCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            app.Command("create", create =>
            {
                create.Description = "Create package template";
                var packagePath = create.Option("--package-path", "Directory where package will be created", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageName = create.Option("--package-name", "Package name.", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageVersion = create.Option("--package-version", "Package version.", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageDestinationDir = create.Option("--package-destination-dir", "Destination dir version.", CommandOptionType.SingleValue)
                    .IsRequired();

                create.OnExecute(async () =>
                {
                    var request = new TemplateCreateRequest(packagePath.Value(), packageName.Value(), packageVersion.Value(), packageDestinationDir.Value());
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
                template.UsePackageCreateCmd(serviceProvider);
            });

            return app;
        }

        public static CommandLineApplication UsePackageCreateCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            app.Command("create", create =>
            {
                create.Description = "Create package template";
                var packageSoureDir = create.Option("--package-sourec-dir", "Directory where package will be taken from", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageName = create.Option("--package-name", "Package name.", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageVersion = create.Option("--package-version", "Package version.", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageInstalationDir = create.Option("--package-instalation-dir", "Instalation dir version.", CommandOptionType.SingleValue)
                     .IsRequired();
                var packageDestinationDir = create.Option("--package-destination-dir", "Destination dir version.", CommandOptionType.SingleValue)
                    .IsRequired();

                create.OnExecute(async () =>
                {
                    var request = new PackageCreateRequest(packageSoureDir.Value(), packageName.Value(), packageVersion.Value(), packageInstalationDir.Value(), packageDestinationDir.Value());
                    var response = await mediator.Send(request);
                    console.WriteLine(response.result);
                });
            });

            return app;
        }
    }
}
