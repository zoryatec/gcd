using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Commands.NipkgPackageBuilserSetVersion;
using Gcd.Extensions;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PackageBuilderSetVersion;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;
public static class UseNipkgPackageBuilderSetPropertyCmdExtensions
{
    public static CommandLineApplication UseNipkgPackageBuilderSetPropertyCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(COMMAND, command =>
        {
            var rootDirOpt = command.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var options = new List<ControlPropertyOption>
            {
                new PackageVersionOption(),
                new PackageHomePageOption(),
                new PackageMaintainerOption(),
                new PackageNameOption(),
            };

            command.AddOptions(options);

            command.OnExecuteAsync(async cancelationToken =>
            {
                var rootDir = PackageBuilderRootDir.Create(rootDirOpt.Value());
                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());

                if (rootDir.IsFailure)
                {
                    console.Error.Write(rootDir.Error);
                    return 1;
                }

                return await properties
                    .Bind((prop) => mediator.PackageBuilderSetPropertiesAsync(rootDir.Value, prop, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}

public abstract class ControlPropertyOption(string template, CommandOptionType optionType) : CommandOption(template, optionType);
public sealed class PackageVersionOption() : ControlPropertyOption($"--{PACKAGE_VERSION_OPTION}", CommandOptionType.SingleValue);
public sealed class PackageNameOption() : ControlPropertyOption(PACKAGE_NAME_OPTION, CommandOptionType.SingleValue);
public sealed class PackageHomePageOption() : ControlPropertyOption($"--{PACKAGE_HOME_PAGE_OPTION}", CommandOptionType.SingleValue);
public sealed class PackageMaintainerOption() : ControlPropertyOption($"--{PACKAGE_MAINTAINER_OPTION}", CommandOptionType.SingleValue);

