using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.Commands.NipkgPackageBuilderInit;
using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.NipkgDownloadFeedMetaData
{
    public static class UseNipkgPackageBuilderInitmdExtensions
    {
        public static CommandLineApplication UseNipkgPackageBuilderInitmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            const string SUCESS_MESSAGE = "Metadata pushed successully";
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
                    var request = new TemplateCreateRequest(
                        PackageContentDir.Create(packagePath.Value()).Value,
                        PackageName.Create(packageName.Value()).Value,
                       PackageVersion.Create(packageVersion.Value()).Value,
                       PackageInstalationDir.Create(packageDestinationDir.Value()).Value);

                    var response = await mediator.Send(request);
                    console.WriteLine(response.result);
                });
            });

            return app;
        }
    }
}

