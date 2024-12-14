using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Commands.NipkgPackageBuilserSetVersion;
using Gcd.Extensions;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using static Gcd.Contract.Nipkg.PackageBuilderSetVersion;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;


public static class UseNipkgPackageBuilderSetVersionCmdExtensions
{
    public static CommandLineApplication UseNipkgPackageBuilderSetVersionCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(COMMAND, command =>
        {
            var packagePathOption = command.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var options = new List<ControlPropertyOption>
            {
                new PackageVersionOption(),
                new PackageHomePageOption(),
                new PackageMaintainerOption()
            };


            command.AddOptions(options);

            command.OnExecuteAsync(async cancelationToken =>
            {
                var packagePath = PackageBuilderRootDir.Create(packagePathOption.Value());

                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());

                return await 
                        properties
                    .Bind((prop) => mediator.PackageBuilderSetPropertiesAsync(packagePath.Value, prop, cancelationToken))
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
public sealed class PackageHomePageOption() : ControlPropertyOption($"--{PACKAGE_HOME_PAGE_OPTION}", CommandOptionType.SingleValue);
public sealed class PackageMaintainerOption() : ControlPropertyOption($"--{PACKAGE_MAINTAINER_OPTION}", CommandOptionType.SingleValue);

