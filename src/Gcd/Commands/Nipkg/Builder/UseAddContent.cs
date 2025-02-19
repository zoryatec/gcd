using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Commands.Nipkg.Builder.AddContent;
using Gcd.Model.Nipkg.PackageBuilder;
using Gcd.Commands.Nipkg;


namespace Gcd.Commands.Nipkg.Builder;

public static class UseAddContentCmdExtensions
{
    public static readonly string NAME = "add-content";
    public const string DESCRIPTION = "Command adds content to 'builder' directory.";
    public static CommandLineApplication UseCmdAddContent(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = serviceProvider.GetRequiredService<IControlPropertyFactory>();

        app.Command(NAME, command =>
        {
            command.Description = DESCRIPTION;
            var contentSrcDirOpt = new BuilderContentSourceDirOption();
            var targetDirOpt = new InatallationTargetRootDirOption();
            var builderRootOpt = new BuilderRootDirOption();

            command.AddOptions(
                builderRootOpt.IsRequired(),
                contentSrcDirOpt.IsRequired(),
                targetDirOpt.IsRequired()
                );


            command.OnExecuteAsync(async cancelationToken =>
            {
                var builderRootDir = builderRootOpt.Map();
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

public class BuilderContentSourceDirOption : CommandOption
{
    public static readonly string NAME = "--content-src-dir";
    public BuilderContentSourceDirOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<PackageBuilderContentSourceDir> Map() =>
        PackageBuilderContentSourceDir.Of(Value());
}

public class InatallationTargetRootDirOption : CommandOption
{
    public static readonly string NAME = "--target-root-dir";
    public InatallationTargetRootDirOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public Result<InatallationTargetRootDir> Map() =>
    InatallationTargetRootDir.Create(Value());
}