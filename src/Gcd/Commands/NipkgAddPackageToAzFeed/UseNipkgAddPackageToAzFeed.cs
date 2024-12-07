using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;

namespace Gcd.Commands.NipkgAddPackageToAzFeed
{
    public static class UseNipkgAddPackageToAzFeedCmdExtensions
    {
        public static CommandLineApplication UseNipkgAddPackageToAzFeedCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            app.Command("add-package-blob-feed", subCmd =>
            {
                subCmd.Description = "Create package template";
                var packagePath = subCmd.Option("--package-path", "File path must en", CommandOptionType.SingleValue).IsRequired();
                var feedUrl = subCmd.Option("--feed-url", "File path must en", CommandOptionType.SingleValue).IsRequired();
                subCmd.OnExecuteAsync(async cancelationToken =>
                {
                    var request = new AddPackageToFeedRequest(feedUrl.Value(), packagePath.Value());
                    var response = await mediator.Send(request);

                    return response
                        .Tap(() => console.Write("Added sucessfuly"))
                        .TapError(error => console.Error.Write(error))
                        .Finally(x => x.IsFailure ? 1 : 0);
                });
            });

            return app;
        }
    }
}




