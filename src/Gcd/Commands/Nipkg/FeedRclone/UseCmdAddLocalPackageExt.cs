using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Extensions;
using Gcd.Model.Nipkg.FeedDefinition;

namespace Gcd.Commands.Nipkg.FeedRclone;

public static class UseCmdAddLocalPackageExt
{
    public static string NAME = "add-local-package";
    public static string DESCRIPTION = "add-local-package";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdAddLocalPackage(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var locPathOpt = new PackageLocalPathOption();
            var feedDirectoryOption = new RcloneFeedDirOption();


            cmd.AddOptions(
                locPathOpt.IsRequired(),
                feedDirectoryOption.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var rcloneFeedDir = feedDirectoryOption.Map();
                
                var feedDefinition =  FeedDefinitionRclone.Of(rcloneFeedDir.Value);
                    // .Bind(feedUri => FeedDefinitionRclone.Of(feedUri));
                
                var pathToPackage = locPathOpt.ToPackageLocalPath();
                var cmdPath = NipkgCmdPath.None;

                return await Result
                    .Combine(feedDefinition, pathToPackage)
                    .Bind(() => mediator.AddPackageToRemoteFeedAsync(feedDefinition.Value, pathToPackage.Value, cmdPath, UseAbsolutePath.No, false,  cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}





