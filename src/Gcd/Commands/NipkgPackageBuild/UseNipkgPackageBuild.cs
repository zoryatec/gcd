using Gcd.Commands.NipkgPackageBuild;
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

        app.Command(COMMAND, create =>
        {
            create.Description = COMMAND_DESCRIPTION;
            var packageSoureDir = create.Option(PACKAGE_CONTENT_DIR_OPTION, PACKAGE_CONTENT_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageName1 = create.Option(PACKAGE_NAME_OPTION, PACKAGE_NAME_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageVersion1 = create.Option(PACKAGE_VERSION_OPTION, PACKAGE_VERSION_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();
            var packageInstalationDir1 = create.Option(PACKAGE_INSTALATION_DIR_OPTION, PACKAGE_INSTALATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                    .IsRequired();
            var packageDestinationDir1 = create.Option(PACKAGE_DESTINATION_DIR_OPTION,PACKAGE_DESTINATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            create.OnExecuteAsync(async cancelationToken =>
            {
                var packageContent = PackageContentDir.Create(packageSoureDir.Value());
                var packageName = PackageName.Create(packageName1.Value());
                var packageVersion = PackageVersion.Create(packageVersion1.Value());
                var packageInstalationDir = PackageInstalationDir.Create(packageInstalationDir1.Value());
                var packageDestinationDir = PackageDestinationDirectory.Create(packageDestinationDir1.Value());

                var request = new PackageBuildRequest(packageContent.Value, packageName.Value, packageVersion.Value, packageInstalationDir.Value, packageDestinationDir.Value);
                var response = await mediator.Send(request, cancelationToken);
                console.WriteLine(response.result);
            });
        });
        return app;
    }
}

