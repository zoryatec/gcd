using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Commands.NipkgPackageBuilserSetVersion;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using static Gcd.Contract.Nipkg.PackageBuilderSetVersion;

namespace Gcd.Commands.NipkgDownloadFeedMetaData
{
    public static class UseNipkgPackageBuilderSetVersionCmdExtensions
    {
        public static CommandLineApplication UseNipkgPackageBuilderSetVersionCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            app.Command(COMMAND, command =>
            {
                var packagePathOption = command.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue)
                    .IsRequired();

                List<CommandOption> propertyOptions = new List<CommandOption>
                {
                    new CommandOption($"--{PACKAGE_VERSION_OPTION}",CommandOptionType.SingleValue),
                    new CommandOption($"--{PACKAGE_HOME_PAGE_OPTION}",CommandOptionType.SingleValue),
                    new CommandOption($"--{PACKAGE_MAINTAINER_OPTION}",CommandOptionType.SingleValue)
                };


                command.AddOptions(propertyOptions);

                command.OnExecuteAsync(async cancelationToken =>
                {
                    var packagePath = PackageBuilderRootDir.Create(packagePathOption.Value());

                    var properties = CreateControlProperties(propertyOptions);

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

        private static Result<IReadOnlyList<ControlFileProperty>> CreateControlProperties(IReadOnlyList<CommandOption> commandOptions)
        {
            List<Result<ControlFileProperty>> controlFileProperties = new List<Result<ControlFileProperty>>();
            var filteredOptions = commandOptions.Where(x => x.HasValue()).ToList() ;

            foreach (var option in filteredOptions)
            {
                controlFileProperties.Add(ControPropertyFactory(option));
            }
            var result = Result.Combine(controlFileProperties);
            if (result.IsFailure) return Result.Failure<IReadOnlyList<ControlFileProperty>>(result.Error);

            return Result.Success(controlFileProperties.Select(x => x.Value).ToList() as IReadOnlyList<ControlFileProperty>);
 
        }

        private static Result<ControlFileProperty> ControPropertyFactory(CommandOption option)
        {

            var result = option switch
            {
                { LongName: PACKAGE_VERSION_OPTION } => PackageVersion.Create(option.Value()).Map(x => x as ControlFileProperty),
                { LongName: PACKAGE_HOME_PAGE_OPTION } => PackageHomePage.Of(option.Value()).Map(x => x as ControlFileProperty),
                { LongName: PACKAGE_MAINTAINER_OPTION } => PackageMaintainer.Of(option.Value()).Map(x => x as ControlFileProperty),
                _ => Result.Failure<ControlFileProperty>($"not implemented factory option {option.LongName}")
            };
            return result;
        }

        public static CommandLineApplication AddOptions(this CommandLineApplication app, IReadOnlyCollection<CommandOption> options)
        {
            foreach (var option in options) app.AddOption(option);
            return app;
        }

    }
}
