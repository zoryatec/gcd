using CSharpFunctionalExtensions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public static class UseNipkgPullFeedMetaCmdExtensions
{
    public static CommandLineApplication UseNipkgPullFeedMetaCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        const string SUCESS_MESSAGE = "Metadata pushed successully";

        app.Command("pull-feed-meta", subCmd =>
        {
            var feedPatht = subCmd.Option("--feed-local-path", "Path to local directory with feed", CommandOptionType.SingleValue).IsRequired();
            var feedUrl = subCmd.Option("--feed-uri", "Link to remote feed", CommandOptionType.SingleValue).IsRequired();
            subCmd.OnExecuteAsync(async cancelationToken =>
            {
                var feedUri = FeedUri.Create(feedUrl.Value());
                var feedPath = FeedPath.Create(feedPatht.Value());

                return await Result
                    .Combine(feedUri, feedPath)
                    .Map(() => new NipkgPullFeedMetaRequest(feedUri.Value, feedPath.Value))
                    .Bind(async (req) => await mediator.Send(req,cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

