﻿
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using static Gcd.Contract.Nipkg.AddPackageToAzFeed;
using Gcd.Model;
using Gcd.Model.Config;
using Gcd.Commands.Nipkg.Builder.Init;
using Gcd.Extensions;
using Gcd.Commands.Nipkg.Feed.AddPackageLocal;

namespace Gcd.Commands.Nipkg.Feed.AddPackageAz;

public static class UseAddHttpPackageCmdExtensions
{
    public static CommandLineApplication UseAddHttpPackageCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-http-package", cmd =>
        {
            cmd.Description = "add local package to local feed";

            var locPathOpt = new PackageHttpPathOption();
            var feedDirOpt = new FeedLocalDirOption();
            var feedCreateOpt = new FeedCreateOption();

            cmd.AddOptions(
                locPathOpt.IsRequired(),
                feedDirOpt.IsRequired(),
                feedCreateOpt
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var locPath = locPathOpt.ToPackageHttpPath();
                var feedDef = feedDirOpt.ToLocalFeedDefinition();
                var cmdPath = NipkgCmdPath.None;
                var feedCreate = feedCreateOpt.IsSet();

                return await Result
                    .Combine(locPath, feedDef)
                    .Bind(() => mediator.AddToLocalFeedAsync(feedDef.Value,locPath.Value,cmdPath, feedCreate, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}




