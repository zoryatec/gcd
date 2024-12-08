using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadFeedMetaData;
using CSharpFunctionalExtensions.ValueTasks;
using static Gcd.Contract.Nipkg.AddPackageToAzFeed;

namespace Gcd.Commands.NipkgAddPackageToAzFeed;

public static class UseNipkgAddPackageToAzFeedCmdExtensions
{
    public static CommandLineApplication UseNipkgAddPackageToAzFeedCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(COMMAND_NAME, subCmd =>
        {
            subCmd.Description = COMMAND_DESCRIPTION;
            var packagePath = subCmd.Option(PACKAGE_PATH_OPTION, PACKAGE_PATH_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
            var feedUrl = subCmd.Option(AZ_FEED_URI_OPTION, AZ_FEED_URI_OPTION_DESCRIPTION, CommandOptionType.SingleValue).IsRequired();
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





