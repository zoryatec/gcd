using Gcd.Commands.Nipkg.Builder.SetProperty;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Model;
using Gcd.Commands.Nipkg.Builder.AddContent;


namespace Gcd.Commands.Nipkg.Builder.Init;

public static class UseAddContentCmdExtensions
{
    public static CommandLineApplication UseAddContentCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command("add-content", command =>
        {
            command.Description = "COMMAND_DESCRIPTION";
            //var builderRootDirOpt = new PackageBuilderRootDirOption();
            var contentSrcDirOpt = new BuilderContentSourceDirOption();
            var targetDirOpt = new InatallationTargetRootDirOption();
            var builderRootDirArg = new PackageBuilderRootDirArgument();
            command.AddArgument(builderRootDirArg.IsRequired());
            command.AddOptions(contentSrcDirOpt.IsRequired(), targetDirOpt.IsRequired());


            command.OnExecuteAsync(async cancelationToken =>
            {
                var value = builderRootDirArg.Value;
                var builderRootDir = builderRootDirArg.Map();
                var contentSrcDir = contentSrcDirOpt.Map();
                var targetDir = targetDirOpt.Map();

                return await Result
                    .Combine(builderRootDir, contentSrcDir, targetDir)
                    .Map(() => ContentLink.Of(targetDir.Value,contentSrcDir.Value))
                    .Bind((link) => mediator.AddContentAsync(builderRootDir.Value, link, cancelationToken))
                    .Tap(() => console.Write("SUCESS_MESSAGE"))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}

public class PackageBuilderRootDirArgument :  CommandArgument
{
    public static readonly string NAME = "--builder-root-dir";
    public PackageBuilderRootDirArgument() : base()
    {
        this.Description = "Description Arg";
    }

    public Result<PackageBuilderRootDir> Map() =>
        PackageBuilderRootDir.Of(this.Value);
}

public class BuilderContentSourceDirOption : CommandOption
{
    public static readonly string NAME = "--content-src-dir";
    public BuilderContentSourceDirOption() : base(NAME, CommandOptionType.SingleValue)
    {
        this.Description = "Description";
    }
    public Result<PackageBuilderContentSourceDir> Map() =>
        PackageBuilderContentSourceDir.Of(this.Value());
}

public class InatallationTargetRootDirOption : CommandOption
{
    public static readonly string NAME = "--target-root-dir";
    public InatallationTargetRootDirOption() : base(NAME, CommandOptionType.SingleValue)
    {
        this.Description = "Description";
    }
    public Result<InatallationTargetRootDir> Map() =>
    InatallationTargetRootDir.Create(this.Value());
}