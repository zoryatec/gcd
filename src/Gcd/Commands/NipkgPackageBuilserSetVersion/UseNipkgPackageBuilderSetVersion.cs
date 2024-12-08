using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.Commands.NipkgPackageBuilserSetVersion;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.NipkgDownloadFeedMetaData
{
    public static class UseNipkgPackageBuilderSetVersionCmdExtensions
    {
        public static CommandLineApplication UseNipkgPackageBuilderSetVersionCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            //const string SUCESS_MESSAGE = "Metadata pushed successully";

            app.Command("set-version", create =>
            {
                create.Description = "Create package template";
                var packagePath = create.Option("--package-path", "Directory where package will be created", CommandOptionType.SingleValue)
                    .IsRequired();
                var packageVersion = create.Option("--package-version", "Package version.", CommandOptionType.SingleValue)
                    .IsRequired();


                create.OnExecute(async () =>
                {
                    var request = new PackageBuilderSetVersionRequest(
                        PackageDestinationDirectory.Create(packagePath.Value()).Value,
                        PackageVersion.Create(packageVersion.Value()).Value);
                    var response = await mediator.Send(request);
                    console.WriteLine(response.result);
                });
            });

            return app;
        }
    }
}
