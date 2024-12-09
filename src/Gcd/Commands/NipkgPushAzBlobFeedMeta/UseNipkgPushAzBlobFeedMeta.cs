using CSharpFunctionalExtensions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.PushAzBlobFeedMetaData;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

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
                var feedUri = AzBlobFeedUri.Create(feedUrl.Value());
                var feedPath = FeedPath.Create(feedPatht.Value());

                return await Result
                    .Combine(feedUri, feedPath)
                    .Map( () => new NipkgPushAzBlobFeedMetaRequest(feedUri.Value, feedPath.Value))
                    .Bind(async (req) => await mediator.Send(req))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}
