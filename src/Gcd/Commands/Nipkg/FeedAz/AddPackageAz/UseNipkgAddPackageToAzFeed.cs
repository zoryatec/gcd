using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using static Gcd.Contract.Nipkg.AddPackageToAzFeed;
using Gcd.Model;
using Gcd.Model.Config;
using Gcd.Model.FeedDefinition;

namespace Gcd.Commands.Nipkg.Feed.AddPackageAz;

public static class UseNipkgAddPackageToAzFeedCmdExtensions
{
    public static CommandLineApplication UseNipkgAddPackageToAzFeedCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(COMMAND_NAME, subCmd =>
        {
            subCmd.Description = COMMAND_DESCRIPTION;
            var packagePathOption = subCmd.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
            var feedUrlOption = subCmd.Option(AZ_FEED_URI_OPTION, AZ_FEED_URI_OPTION_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
            subCmd.OnExecuteAsync(async cancelationToken =>
            {
                var azFeedDef = AzBlobFeedUri.Create(feedUrlOption.Value())
                    .Bind(feedUri => FeedDefinitionAzBlob.Of(feedUri));
                var pathToPackage = PackageFilePath.Of(packagePathOption.Value());
                var cmdPath = NipkgCmdPath.None;

                return await Result
                    .Combine(azFeedDef, pathToPackage)
                    .Map(() => new AddPackageToAzFeedRequest(azFeedDef.Value, pathToPackage.Value, cmdPath))
                    .Bind((req1) => mediator.Send(req1, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}





