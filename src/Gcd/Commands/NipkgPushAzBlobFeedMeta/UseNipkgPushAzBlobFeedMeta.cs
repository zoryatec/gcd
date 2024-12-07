using CSharpFunctionalExtensions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.NipkgDownloadFeedMetaData
{
    public static class UUseNipkgPushAzBlobFeedMetaaCmdExtensions
    {
        const string COMMAND_NAME = "push-feed-meta";

        const string LOCAL_FEED_PATH_OPTION = "--feed-local-path";
        const string LOCAL_FEED_PATH_OPTION_DESCRIPTION = "Path to local directory with feed";

        const string RMOTE_FEED_URI_OPTION = "--feed-uri";
        const string REMOTE_FEED_PATH_OPTION_DESCRIPTION = "Uri to reomte az blob feed where meta data will be pushed";

        const string SUCESS_MESSAGE = "Metadata pushed successully";

        public static CommandLineApplication UseNipkgPushAzBlobFeedMetaCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            app.Command(COMMAND_NAME, subCmd =>
            {
                var feedPatht = subCmd.Option(LOCAL_FEED_PATH_OPTION, LOCAL_FEED_PATH_OPTION_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
                var feedUrl = subCmd.Option(RMOTE_FEED_URI_OPTION, REMOTE_FEED_PATH_OPTION_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
                subCmd.OnExecuteAsync(async cancelationToken =>
                {

                    var feedUri = FeedUri.Create(feedUrl.Value());
                    var feedPath = FeedPath.Create(feedPatht.Value());


                    return await Result
                        .Combine(feedUri, feedPath)
                        .TapError(error => console.Error.Write(error))
                        .Map( () => new NipkgPushAzBlobFeedMetaRequest(feedUri.Value, feedPath.Value))
                        .Tap(async (req) => await mediator.Send(req))
                        .Tap(() => console.Write(SUCESS_MESSAGE))
                        .TapError(error => console.Error.Write(error))
                        .Finally(x => x.IsFailure ? 1 : 0);
                    


                    //if (result.IsFailure)
                    //{
                    //    console.Error.Write(result.Error);
                    //    return 1;
                    //}



                    //return response
                    //    .Tap(() => console.Write(SUCESS_MESSAGE))
                    //    .TapError(error => console.Error.Write(error))
                    //    .Finally(x => x.IsFailure ? 1 : 0);
                });
            });
            return app;
        }
    }
}