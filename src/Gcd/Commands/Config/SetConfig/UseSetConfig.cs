using Gcd.Commands.Nipkg.Builder.SetProperty;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Model;
using Gcd.Commands.Nipkg.Builder.AddContent;
using Gcd.Commands.Nipkg.Builder.Init;

namespace Gcd.Commands.Config.SetConfig;

public static class UseSetConfigCmdExtensions
{
    public static CommandLineApplication UseSetConfigCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command("add-content", command =>
        {
            command.Description = "COMMAND_DESCRIPTION";
            var contentSrcDirOpt = new BuilderContentSourceDirOption();
            var targetDirOpt = new InatallationTargetRootDirOption();
            var builderRootDirArg = new PackageBuilderRootDirArgument();
            command.AddArgument(builderRootDirArg.IsRequired());
            command.AddOptions(contentSrcDirOpt.IsRequired(), targetDirOpt.IsRequired());


            command.OnExecuteAsync(async cancelationToken =>
            {
                var builderRootDir = builderRootDirArg.Map();
                var contentSrcDir = contentSrcDirOpt.Map();
                var targetDir = targetDirOpt.Map();

                return await Result
                    .Combine(builderRootDir, contentSrcDir, targetDir)
                    .Map(() => ContentLink.Of(targetDir.Value, contentSrcDir.Value))
                    .Bind((link) => mediator.AddContentAsync(builderRootDir.Value, link, cancelationToken))
                    .Tap(() => console.Write("SUCESS_MESSAGE"))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}



public class NipkgInstallerUriOption : CommandOption
{
    public static readonly string NAME = "--nipkg-installer-uri";
    public NipkgInstallerUriOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<PackageBuilderContentSourceDir> Map() =>
        PackageBuilderContentSourceDir.Of(Value());
}

public class NipkgCmdPathOption : CommandOption
{
    public static readonly string NAME = "--nipkg-path";
    public NipkgCmdPathOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<InatallationTargetRootDir> Map() =>
    InatallationTargetRootDir.Create(Value());
}