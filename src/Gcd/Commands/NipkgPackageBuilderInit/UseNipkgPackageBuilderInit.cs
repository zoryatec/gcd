using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuilderInit;
using Gcd.Model;
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
            var packagePathOption = create.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageNameOption = create.Option(PACKAGE_NAME_OPTION, PACKAGE_NAME_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageVersionOption = create.Option(PACKAGE_VERSION_OPTION, PACKAGE_VERSION_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageDestinationDirOption = create.Option(PACKAGE_DESTINATION_DIR_OPTION, PACKAGE_DESTINATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            create.OnExecuteAsync(async cancelationToken =>
            {
                var packagePath = PackageContentDir.Create(packagePathOption.Value());
                var packageName = PackageName.Create(packageNameOption.Value());
                var packageVersion = PackageVersion.Create(packageVersionOption.Value());
                var packageDestination = PackageInstalationDir.Create(packageDestinationDirOption.Value());

                return await Result
                    .Combine(packagePath, packageName, packageVersion, packageDestination)
                    .Map(() => new PackageBuilderInitRequest(packagePath.Value, packageName.Value, packageVersion.Value, packageDestination.Value))
                    .Bind((req1) => mediator.Send(req1, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}


