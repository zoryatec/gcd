using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using CSharpFunctionalExtensions.ValueTasks;

namespace Gcd.Commands.NipkgAddPackageToAzFeed
{
    public static class UseNipkgAddPackageToAzFeedCmdExtensions
    {
        public static CommandLineApplication UseNipkgAddPackageToAzFeedCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
        {
            const string SUCESS_MESSAGE = "Metadata pushed successully";

            var console = serviceProvider.GetRequiredService<IConsole>();
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            app.Command("add-package-blob-feed", subCmd =>
            {
                subCmd.Description = "Create package template";
                var packagePath = subCmd.Option("--package-path", "File path must en", CommandOptionType.SingleValue).IsRequired();
                var feedUrl = subCmd.Option("--feed-url", "File path must en", CommandOptionType.SingleValue).IsRequired();
                subCmd.OnExecuteAsync(async cancelationToken =>
                {
                    var feedUri = FeedUri.Create(feedUrl.Value());
                    var pathToPackage = PackagePath.Create(packagePath.Value());

                    return await Result
                        .Combine(feedUri, pathToPackage)
                        .Map(() => new AddPackageToFeedRequest(feedUri.Value, pathToPackage.Value))
                        .Bind((req1) =>  mediator.Send(req1))
                        .Tap(() => console.Write(SUCESS_MESSAGE))
                        .TapError(error => console.Error.Write(error))
                        .Finally(x => x.IsFailure ? 1 : 0);
                });
            });

            return app;
        }
    }
}




