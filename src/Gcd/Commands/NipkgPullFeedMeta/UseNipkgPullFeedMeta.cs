using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Commands.NipkgDownloadFeedMetaData
{
    public static class UseNipkgPullFeedMetaCmdExtensions
    {
        public static CommandLineApplication UseNipkgPullFeedMetaCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            app.Command("pull-feed-meta", subCmd =>
            {
                var feedPath = subCmd.Option("--feed-local-path", "Path to local directory with feed", CommandOptionType.SingleValue).IsRequired();
                var feedUrl = subCmd.Option("--feed-uri", "Link to remote feed", CommandOptionType.SingleValue).IsRequired();
                subCmd.OnExecute(async () =>
                {
                    var request = new DownloadFeedMetadataRequest(feedUrl.Value(), feedPath.Value());
                    var response = await mediator.Send(request);
                    console.WriteLine(response.Result);
                });
            });
            return app;
        }
    }
    }
