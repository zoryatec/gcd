﻿using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Builder;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PackageBuilderSetProperty;

namespace Gcd.Commands.Nipkg.Builder;
public static class UseCmdSetPropertyExt
{
    public static readonly string NAME = "set-property";
    public static readonly string DESCRIPTION = "set-property";
    public static readonly string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdSetProperty(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(NAME, command =>
        {
            command.Description = DESCRIPTION;

            var rootDirOpt = new BuilderRootDirOption();

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

            command.AddOptions(options);
            command.AddOptions(rootDirOpt.IsRequired());

            command.OnExecuteAsync(async cancelationToken =>
            {
                var rootDir = rootDirOpt.Map();
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

public sealed class PackageArchitectureOption() : ControlPropertyOption(PACKAGE_ARCHITECTURE_OPTION, CommandOptionType.SingleValue);
public sealed class PackageHomePageOption() : ControlPropertyOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-home-page";
}
public sealed class PackageMaintainerOption() : ControlPropertyOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-maintainer";
}
public sealed class PackageDescriptionOption() : ControlPropertyOption(PACKAGE_DESCRIPTION_OPTION, CommandOptionType.SingleValue);
public sealed class PackageXbPluginOption() : ControlPropertyOption(PACKAGE_XB_PLUGIN_OPTION, CommandOptionType.SingleValue);
public sealed class PackageXbUserVisibleOption() : ControlPropertyOption(PACKAGE_XB_USER_VISIBLE_OPTION, CommandOptionType.SingleValue);
public sealed class PackageXbStoreProductOption() : ControlPropertyOption(PACKAGE_XB_STORE_PRODUCT_OPTION, CommandOptionType.SingleValue);
public sealed class PackageXBSectionOption() : ControlPropertyOption(PACKAGE_XB_SECTION_OPTION, CommandOptionType.SingleValue);
public sealed class PackageVersionOption() : ControlPropertyOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-version";
}
public sealed class PackageNameOption() : ControlPropertyOption(NAME, CommandOptionType.SingleValue)
{
    public static readonly string NAME = "--package-name";
}
public sealed class PackageDependenciesOption() : ControlPropertyOption(PACKAGE_DEPENDENCIES_OPTION, CommandOptionType.SingleValue);