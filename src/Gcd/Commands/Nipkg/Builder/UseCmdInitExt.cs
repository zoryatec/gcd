using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Builder;
using Gcd.LocalFileSystem.Abstractions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SMBLibrary;

namespace Gcd.Commands.Nipkg.Builder;

public static class UseCmdInitExt
{
    public static readonly string NAME = "init";
    public static readonly string DESCRIPTION = "Command initializes 'builder' directory with appropriate structure and default files";
    public static readonly string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdInit(this CommandLineApplication app, IServiceProvider serviceProvider)
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

            var instrFileOption = new InstructionFileSourceOption();
            var controlFileOption = new ControlFileSourceOption();

            command.AddOptions(options);
            command.AddOptions(
                rootDirOpt.IsRequired(),
                instrFileOption,
                controlFileOption
            );

            command.OnExecuteAsync(async cancelationToken =>
            {
                var rootDir = rootDirOpt.Map();
                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());

                var controlFilePath = Maybe<LocalFilePath>.None;
                var instructionFilePath = Maybe<LocalFilePath>.None;


                // very ugly but will do for now
                var instFileRes = instrFileOption.Map();
                if(instFileRes.HasValue)
                {
                    var result = instFileRes.Value;
                    if(result.IsFailure)
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
                    .Combine(rootDir, properties)
                    .Bind(() => mediator.PackageBuilderInitAsync(rootDir.Value, properties.Value,instructionFilePath,controlFilePath, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}


