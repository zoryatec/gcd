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
                create.Description = COMMAND_DESCRIPTION;
                var packagePathOption = create.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                    .IsRequired();
                var packageVersionOption = create.Option(PACKAGE_VERSION_OPTION, PACKAGE_VERSION_DESCRIPTION, CommandOptionType.SingleValue)
                    .IsRequired();

                create.OnExecuteAsync(async cancelationToken =>
                {
                    var packagePath = PackageDestinationDirectory.Create(packagePathOption.Value());
                    var packageVersion = PackageVersion.Create(packageVersionOption.Value());

                    return await Result
                        .Combine(packagePath, packageVersion)
                        .Map(() => new PackageBuilderSetVersionRequest(packagePath.Value, packageVersion.Value))
                        .Bind((req1) => mediator.Send(req1, cancelationToken))
                        .Tap(() => console.Write(SUCESS_MESSAGE))
                        .TapError(error => console.Error.Write(error))
                        .Finally(x => x.IsFailure ? 1 : 0);
                });
            });

            return app;
        }
    }
}
