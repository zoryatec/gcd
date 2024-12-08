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
            //const string SUCESS_MESSAGE = "Metadata pushed successully";

            app.Command("create", create =>
            {
                create.Description = "Create package template";
                var packageSoureDir = create.Option("--package-sourec-dir", "Directory where package will be taken from", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageName1 = create.Option("--package-name", "Package name.", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageVersion1 = create.Option("--package-version", "Package version.", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageInstalationDir1 = create.Option("--package-instalation-dir", "Instalation dir version.", CommandOptionType.SingleValue)
                     .IsRequired();
                var packageDestinationDir1 = create.Option("--package-destination-dir", "Destination dir version.", CommandOptionType.SingleValue)
                    .IsRequired();

                create.OnExecute(async () =>
                {
                    var packageContent = PackageContentDir.Create(packageSoureDir.Value());
                    var packageName = PackageName.Create(packageName1.Value());
                    var packageVersion = PackageVersion.Create(packageVersion1.Value());
                    var packageInstalationDir = PackageInstalationDir.Create(packageInstalationDir1.Value());
                    var packageDestinationDir = PackageDestinationDirectory.Create(packageDestinationDir1.Value());

                    var request = new PackageBuildRequest(packageContent.Value, packageName.Value, packageVersion.Value, packageInstalationDir.Value, packageDestinationDir.Value);
                    var response = await mediator.Send(request);
                    console.WriteLine(response.result);
                });
            });

            return app;
        }
    }
}
