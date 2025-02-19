
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.FeedLocal;

namespace Gcd.Commands.Nipkg.FeedLocal;

public static class UseCmdAddHttpPackageExt
{
    public static readonly string NAME = "add-http-package";
    public static readonly string DESCRIPTION = "add-http-package";
    public static readonly string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdAddHttpPackage(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = "add local package to local feed";

            var locPathOpt = new PackageHttpPathOption();
            var feedDirOpt = new FeedLocalDirOption();
            var feedCreateOpt = new FeedCreateOption();
            var useAbsPathOpt = new UseAbsolutePathOption();

            cmd.AddOptions(
                locPathOpt.IsRequired(),
                feedDirOpt.IsRequired(),
                feedCreateOpt,
                useAbsPathOpt
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var locPath = locPathOpt.ToPackageHttpPath();
                var feedDef = feedDirOpt.ToLocalFeedDefinition();
                var cmdPath = NipkgCmdPath.None;
                var useAbsPath = useAbsPathOpt.Map();
                var feedCreate = feedCreateOpt.IsSet();

                return await Result
                    .Combine(locPath, feedDef)
                    .Bind(() => mediator.AddToLocalFeedAsync(feedDef.Value, locPath.Value, cmdPath, useAbsPath, feedCreate, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}





