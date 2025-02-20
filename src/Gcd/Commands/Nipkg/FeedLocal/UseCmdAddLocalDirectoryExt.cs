using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.FeedLocal;

namespace Gcd.Commands.Nipkg.FeedLocal;

public static class UseCmdAddLocalDirectoryExt
{
    public static string NAME = "add-local-directory";
    public static string DESCRIPTION = "add-local-directory";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdAddLocalDirectory(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;

            var locDirOpt = new PackageLocalDirectoryOption();
            var feedDirOpt = new FeedLocalDirOption();
            var feedCreateOpt = new FeedCreateOption();

            cmd.AddOptions(
                locDirOpt.IsRequired(),
                feedDirOpt.IsRequired(),
                feedCreateOpt
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var locDirPath = locDirOpt.Map();
                var feedDef = feedDirOpt.ToLocalFeedDefinition();
                var cmdPath = NipkgCmdPath.None;
                var feedCreate = feedCreateOpt.IsSet();

                return await mediator.AddLocalDirAsync(feedDef.Value, locDirPath.Value, cmdPath, UseAbsolutePath.No, feedCreate, cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}





