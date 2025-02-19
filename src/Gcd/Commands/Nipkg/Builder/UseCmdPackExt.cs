using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Builder;
using Gcd.Model.Config;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.Nipkg.Builder;

public static class UseCmdPackExt
{
    public static readonly string NAME = "pack";
    public static readonly string DESCRIPTION = "Command is a proxy for 'nipkg pack.";
    public static readonly string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdPack(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var packageDestinationOption = new PackageDestinationDirOption();
            var builderRootDirOpt = new BuilderRootDirOption();

            cmd.AddOptions(
                builderRootDirOpt.IsRequired(),
                packageDestinationOption.IsRequired()
                );


            cmd.OnExecuteAsync(async cancellationToken =>
            {
                var packageDestinationDir = packageDestinationOption.Map();
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

