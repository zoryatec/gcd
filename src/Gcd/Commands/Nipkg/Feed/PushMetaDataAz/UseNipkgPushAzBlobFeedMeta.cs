﻿using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model;
using Gcd.Model.File;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PushAzBlobFeedMetaData;

namespace Gcd.Commands.Nipkg.Feed.PushMetaDataAz;

public static class UUseNipkgPushAzBlobFeedMetaaCmdExtensions
{
    public static CommandLineApplication UseNipkgPushAzBlobFeedMetaCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(COMMAND_NAME, subCmd =>
        {
            subCmd.Description = COMMAND_DESCRIPTION;
            var feedPatht = subCmd.Option(LOCAL_FEED_PATH_OPTION, LOCAL_FEED_PATH_OPTION_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
            var feedUrl = subCmd.Option(REMOTE_FEED_URI_OPTION, REMOTE_FEED_PATH_OPTION_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
            subCmd.OnExecuteAsync(async cancelationToken =>
            {
                var azFeedDef = AzBlobFeedUri.Create(feedUrl.Value())
                    .Bind(feedUri => AzBlobFeedDefinition.Of(feedUri));

                var localFeedDef = LocalDirPath.Parse(feedPatht.Value())
                    .Bind(feedPath => LocalFeedDefinition.Of(feedPath));

                return await Result
                    .Combine(azFeedDef, localFeedDef)
                    .Bind(() => mediator.PushAzBlobFeedMetaDataAsync(azFeedDef.Value, localFeedDef.Value, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}
