using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Extensions;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PackageBuilderInit;


namespace Gcd.Commands.Nipkg.Builder.Init;

public static class UseNipkgPackageBuilderInitmdExtensions
{
    public static CommandLineApplication UseNipkgPackageBuilderInitmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(COMMAND, command =>
        {
            command.Description = COMMAND_DESCRIPTION;
            var rootDirOpt = command.Option(PACKAGE_BUILDER_DIR_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
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

            command.AddOptions(options);

            command.OnExecuteAsync(async cancelationToken =>
            {
                var rootDir = PackageBuilderRootDir.Of(rootDirOpt.Value());
                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());

                return await Result
                    .Combine(rootDir, properties)
                    .Bind(() => mediator.PackageBuilderInitAsync(rootDir.Value, properties.Value, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}


