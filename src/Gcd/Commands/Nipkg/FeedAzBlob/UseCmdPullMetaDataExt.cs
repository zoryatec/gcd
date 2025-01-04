using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model;
using Gcd.Model.File;
using Gcd.Model.Nipkg.FeedDefinition;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PullFeedMetaData;

namespace Gcd.Commands.Nipkg.FeedAzBlob;

public static class UseCmdPullMetaDataExt
{
    public static string NAME = "pull-meta-data";
    public static string DESCRIPTION = "pull-meta-data";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdPullMetaData(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;

            var feedPathOpt = new FeedLocalDirOption();
            var feedUrl = cmd.Option(REMOTE_FEED_URI_OPTION, REMOTE_FEED_URI_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();

            cmd.AddOptions(
                feedPathOpt.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var azFeedDef = AzBlobFeedUri.Create(feedUrl.Value())
                    .Bind(feedUri => FeedDefinitionAzBlob.Of(feedUri));

                var localFeedDef = LocalDirPath.Parse(feedPathOpt.Value())
                    .Bind(feedPath => FeedDefinitionLocal.Of(feedPath));

                return await Result
                    .Combine(azFeedDef, localFeedDef)
                    .Bind(() => mediator.PullFeedMetaAsync(azFeedDef.Value, localFeedDef.Value, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

