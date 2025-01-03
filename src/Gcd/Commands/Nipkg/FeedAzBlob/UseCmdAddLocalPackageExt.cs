using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using static Gcd.Contract.Nipkg.AddPackageToAzFeed;
using Gcd.Model;
using Gcd.Model.Config;
using Gcd.Model.FeedDefinition;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Extensions;

namespace Gcd.Commands.Nipkg.FeedAzBlob;

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
            var feedUrlOption = cmd.Option(AZ_FEED_URI_OPTION, AZ_FEED_URI_OPTION_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();


            cmd.AddOptions(
                locPathOpt.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var azFeedDef = AzBlobFeedUri.Create(feedUrlOption.Value())
                    .Bind(feedUri => FeedDefinitionAzBlob.Of(feedUri));
                var pathToPackage = locPathOpt.ToPackageLocalPath();
                var cmdPath = NipkgCmdPath.None;

                return await Result
                    .Combine(azFeedDef, pathToPackage)
                    .Bind(() => mediator.AddPackageToRemoteFeedAsync(azFeedDef.Value, pathToPackage.Value, cmdPath, UseAbsolutePath.No, false,  cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}





