using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.NipkgDownloadFeedMetaData
{
    public static class UseNipkgPackageBuildCmdExtensions
    {
        public static CommandLineApplication UseNipkgPackageBuildCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            const string SUCESS_MESSAGE = "Metadata pushed successully";

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
