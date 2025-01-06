using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Nipkg.FeedDefinition;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
            var feedUrl = new AzFeedUrlOption();

            cmd.AddOptions(
                feedPathOpt.IsRequired(),
                feedUrl.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var azFeedDef = feedUrl.Map()
                    .Bind(feedUri => FeedDefinitionAzBlob.Of(feedUri));

                var localFeedDef = LocalDirPath.Of(feedPathOpt.Value()).MapError(er => er.Message)
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

