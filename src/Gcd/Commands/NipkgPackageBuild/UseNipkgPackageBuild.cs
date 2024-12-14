using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.Commands.NipkgPackageBuilserSetVersion;
using Gcd.Extensions;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PackageBuild;


namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public static class UseNipkgPackageBuildCmdExtensions
{
    public static CommandLineApplication UseNipkgPackageBuildCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(COMMAND, cmd =>
        {
            cmd.Description = COMMAND_DESCRIPTION;
            var packageSoureDirOption = cmd
                .Option(PACKAGE_CONTENT_DIR_OPTION, PACKAGE_CONTENT_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var packageInstalationOption = cmd
                .Option(PACKAGE_INSTALATION_DIR_OPTION, PACKAGE_INSTALATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var packageDestinationOption = cmd
                .Option(PACKAGE_DESTINATION_DIR_OPTION,PACKAGE_DESTINATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var options = new List<ControlPropertyOption>
            {
                new PackageArchitectureOption(),
                new PackageHomePageOption(),
                new PackageMaintainerOption(),
                new PackageDescriptionOption(),
                new PackageXbPluginOption(),
                new PackageXbUserVisibleOption(),
                new PackageXbStoreProductOption(),
                new PackageXBSectionOption(),
                new PackageVersionOption(),
                new PackageNameOption(),
                new PackageDependenciesOption(),
            };

            cmd.AddOptions(options);

            cmd.OnExecuteAsync(async cancellationToken =>
            {
                var packageContent = PackageBuilderRootDir.Create(packageSoureDirOption.Value());
                var packageInstalationDir = PackageInstalationDir.Create(packageInstalationOption.Value());
                var packageDestinationDir = PackageDestinationDirectory.Create(packageDestinationOption.Value());
                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());

                return await Result
                    .Combine(packageContent, packageInstalationDir, packageDestinationDir, properties)
                    .Bind(() => mediator.PackageBuilderBuildAsync(packageContent.Value, packageInstalationDir.Value, packageDestinationDir.Value,properties.Value, cancellationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

