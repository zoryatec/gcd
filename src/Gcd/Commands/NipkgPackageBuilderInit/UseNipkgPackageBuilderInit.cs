using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.Commands.NipkgPackageBuilderInit;
using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PackageBuilderInit;


namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public static class UseNipkgPackageBuilderInitmdExtensions
{
    public static CommandLineApplication UseNipkgPackageBuilderInitmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(COMMAND, create =>
        {
            create.Description = COMMAND_DESCRIPTION;
            var packagePath = create.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageName = create.Option(PACKAGE_NAME_OPTION, PACKAGE_NAME_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageVersion = create.Option(PACKAGE_VERSION_OPTION, PACKAGE_VERSION_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageDestinationDir = create.Option(PACKAGE_DESTINATION_DIR_OPTION, PACKAGE_DESTINATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            create.OnExecute(async () =>
            {
                var request = new PackageBuilderInitRequest(
                    PackageContentDir.Create(packagePath.Value()).Value,
                    PackageName.Create(packageName.Value()).Value,
                    PackageVersion.Create(packageVersion.Value()).Value,
                    PackageInstalationDir.Create(packageDestinationDir.Value()).Value);

                var response = await mediator.Send(request);
                if (response.IsFailure) console.Error.Write(response.Error);
                else console.Out.Write(SUCESS_MESSAGE);
            });
        });

        return app;
    }
}


