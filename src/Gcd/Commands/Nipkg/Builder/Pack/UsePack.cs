

using CSharpFunctionalExtensions;
using Gcd.Commands.Nipkg.Builder.Init;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Builder;
using Gcd.Model;
using Gcd.Model.Config;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PackageBuild;


namespace Gcd.Commands.Nipkg.Build;

public static class UsePackCmdExtensions
{
    public static CommandLineApplication UsePackCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command("pack", cmd =>
        {
            var packageDestinationOption = cmd
                .Option(PACKAGE_DESTINATION_DIR_OPTION, PACKAGE_DESTINATION_DIR_DESCRIPTION, CommandOptionType.SingleValue)
                .IsRequired();

            var builderRootDirOpt = new PackageBuilderRootDirOption();

            cmd.AddOption(builderRootDirOpt);


            cmd.OnExecuteAsync(async cancellationToken =>
            {
                var packageDestinationDir = PackageDestinationDirectory.Of(packageDestinationOption.Value());

                var rootDir = builderRootDirOpt.Map();

                var cmdPath = NipkgCmdPath.None;

                return await Result
                    .Combine(packageDestinationDir, rootDir)
                    .Bind(() => mediator.BuilderPackAsync(rootDir.Value, packageDestinationDir.Value, cmdPath, cancellationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

