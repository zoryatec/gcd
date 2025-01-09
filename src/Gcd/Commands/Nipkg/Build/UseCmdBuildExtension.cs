using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Build;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.Nipkg.Build;

public static class UseCmdBuildExtension
{
    public const string NAME = "build";
    public const string DESCRIPTION = "build a package";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdBuild(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var packageSoureDirOption = new BuilderContentSourceDirOption();
            var packageInstalationOption = new InatallationTargetRootDirOption();
            var packageDestinationDirOpt = new PackageDestinationDirOption();
 

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
            var instrFileOption = new InstructionFileSourceOption();
            var controlFileOption = new ControlFileSourceOption();

            cmd.AddOptions(options);
            cmd.AddOptions(
                packageSoureDirOption.IsRequired(),
                packageDestinationDirOpt.IsRequired(),
                packageInstalationOption.IsRequired(),
                instrFileOption,
                controlFileOption
                );


            cmd.OnExecuteAsync(async cancellationToken =>
            {
                var packageContent = packageSoureDirOption.Map();
                var packageInstalationDir = packageInstalationOption.Map();
                var packageDestinationDir = packageDestinationDirOpt.Map();
                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());
                var cmdPath = NipkgCmdPath.None;

                var controlFilePath = Maybe<LocalFilePath>.None;
                var instructionFilePath = Maybe<LocalFilePath>.None;


                // very ugly but will do for now
                var instFileRes = instrFileOption.Map();
                if (instFileRes.HasValue)
                {
                    var result = instFileRes.Value;
                    if (result.IsFailure)
                    {
                        console.Error.Write(result.Error);
                        return 1;
                    }
                    else
                    {
                        instructionFilePath = result.Value;
                    }
                }

                var contrFileRes = controlFileOption.Map();
                if (contrFileRes.HasValue)
                {
                    var result = contrFileRes.Value;
                    if (result.IsFailure)
                    {
                        console.Error.Write(result.Error);
                        return 1;
                    }
                    else
                    {
                        controlFilePath = result.Value;
                    }
                }

                return await Result
                    .Combine(packageContent, packageInstalationDir, packageDestinationDir, properties)
                    .Bind(() => mediator.PackageBuilderBuildAsync(packageContent.Value, packageInstalationDir.Value, packageDestinationDir.Value, properties.Value, cmdPath, instructionFilePath,controlFilePath, cancellationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

