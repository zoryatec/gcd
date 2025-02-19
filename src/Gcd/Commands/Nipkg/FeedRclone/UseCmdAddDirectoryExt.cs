using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.FeedDefinition;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedRclone;

public static class UseCmdAddDirectoryExt
{
    public static string NAME = "add-local-directory";
    public static string DESCRIPTION = "add-local-directory";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdAddDirectory(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var locDirOpt = new PackageLocalDirectoryOption();
            var feedDirectoryOption = new RcloneFeedDirOption();

            cmd.AddOptions(
                locDirOpt.IsRequired(),
                feedDirectoryOption.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var rcloneFeedDir = feedDirectoryOption.Map();
                var feedDir = locDirOpt.Map();
                
                var feedDefinition =  FeedDefinitionRclone.Of(rcloneFeedDir.Value);
                var cmdPath = NipkgCmdPath.None;

                // this should be also aserted: pathToDir !!!!!!! refactor
                return await feedDefinition
                    .Bind((feedDef) => mediator.AddDirToRemoteFeedAsync(feedDef, feedDir.Value, cmdPath, UseAbsolutePath.No, false,  cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}
