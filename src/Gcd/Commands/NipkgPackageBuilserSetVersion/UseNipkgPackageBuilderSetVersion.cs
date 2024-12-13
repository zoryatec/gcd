using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgPackageBuilserSetVersion;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PackageBuilderSetVersion;

namespace Gcd.Commands.NipkgDownloadFeedMetaData
{
    public static class UseNipkgPackageBuilderSetVersionCmdExtensions
    {
        public static CommandLineApplication UseNipkgPackageBuilderSetVersionCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            app.Command(COMMAND, create =>
            {
                var packagePathOption = create.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                    .IsRequired();
                var packageVersionOption = create.Option(PACKAGE_VERSION_OPTION, PACKAGE_VERSION_DESCRIPTION, CommandOptionType.SingleValue);
                var packageHomePageOption = create.Option(PACKAGE_HOME_PAGE_OPTION, PACKAGE_HOME_PAGE_DESCRIPTION, CommandOptionType.SingleValue);
                var packageMaintainerOption = create.Option(PACKAGE_MAINTAINER_OPTION, PACKAGE_MAINTAINER_DESCRIPTION, CommandOptionType.SingleValue);




                create.OnExecuteAsync(async cancelationToken =>
                {

                    //packageHomePageOption.HasValue
                    var packagePath = PackageBuilderRootDir.Create(packagePathOption.Value());
                    var packageVersion = PackageVersion.Create(packageVersionOption.Value());
                    var packagePage = PackageHomePage.Of(packageHomePageOption.Value());
                    var packageMaintainer = PackageMaintainer.Of(packageMaintainerOption.Value());

                    var result = Result.Combine(packagePath, packageVersion, packagePage, packageMaintainer);

                    result.TapError(error => console.Error.Write(error));

                    IReadOnlyList<ControlFileProperty> properties = new List<ControlFileProperty> { packageMaintainer.Value, packageVersion.Value, packagePage.Value };

                    if (result.IsFailure) return 1;

                    return await Result
                        .Combine(packagePath, packageVersion)
                        .Bind(() => mediator.PackageBuilderSetPropertiesAsync(packagePath.Value, properties, cancelationToken))
                        .Tap(() => console.Write(SUCESS_MESSAGE))
                        .TapError(error => console.Error.Write(error))
                        .Finally(x => x.IsFailure ? 1 : 0);
                });
            });

            return app;
        }
    }



}
