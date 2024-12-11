using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuild;
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

        app.Command(COMMAND, cmd =>
        {
            cmd.Description = COMMAND_DESCRIPTION;
            var packageSoureDirOption = cmd
                .Option(PACKAGE_CONTENT_DIR_OPTION, PACKAGE_CONTENT_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var packageNameOption = cmd
                .Option(PACKAGE_NAME_OPTION, PACKAGE_NAME_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var packageVersionOption = cmd
                .Option(PACKAGE_VERSION_OPTION, PACKAGE_VERSION_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var packageInstalationOption = cmd
                .Option(PACKAGE_INSTALATION_DIR_OPTION, PACKAGE_INSTALATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var packageDestinationOption = cmd
                .Option(PACKAGE_DESTINATION_DIR_OPTION,PACKAGE_DESTINATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var packageContent = PackageBuilderRootDir.Create(packageSoureDirOption.Value());
                var packageName = PackageName.Create(packageNameOption.Value());
                var packageVersion = PackageVersion.Create(packageVersionOption.Value());
                var packageInstalationDir = PackageInstalationDir.Create(packageInstalationOption.Value());
                var packageDestinationDir = PackageDestinationDirectory.Create(packageDestinationOption.Value());

                return await Result
                    .Combine(packageContent, packageName, packageVersion, packageInstalationDir, packageDestinationDir)
                    .Map(() => new PackageBuildRequest(packageContent.Value, packageName.Value, packageVersion.Value, packageInstalationDir.Value, packageDestinationDir.Value))
                    .Bind((req1) => mediator.Send(req1, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

