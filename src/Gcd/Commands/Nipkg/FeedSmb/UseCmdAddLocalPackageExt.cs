﻿using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using static Gcd.Contract.Nipkg.AddPackageToAzFeed;
using Gcd.Model;
using Gcd.Model.Config;
using Gcd.Model.FeedDefinition;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Extensions;

namespace Gcd.Commands.Nipkg.FeedSmb;

public static class UseCmdAddLocalPackageExt
{
    public static CommandLineApplication UseCmdAddLocalPackage(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-local-package", cmd =>
        {
            cmd.Description = COMMAND_DESCRIPTION;
            var locPathOpt = new PackageLocalPathOption();
            var smbShareOpt = new SmbShareAddressOption();
            var smbUserOpt = new SmbUserNameOption();
            var smbPasswordOpt = new SmbPasswordOption();
            var useAbsOpt = new UseAbsolutePathOption();
            var feedCreateOpt = new FeedCreateOption();

            cmd.AddOptions(
                locPathOpt.IsRequired(),
                smbShareOpt.IsRequired(),
                smbUserOpt.IsRequired(),
                smbPasswordOpt.IsRequired(),
                useAbsOpt,
                feedCreateOpt
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var smbShare = smbShareOpt.Map();
                var smbUser = smbUserOpt.Map();
                var smbPassword = smbPasswordOpt.Map();
                var pathToPackage = locPathOpt.ToPackageLocalPath();
                var cmdPath = NipkgCmdPath.None;
                var useAbs = useAbsOpt.Map();
                var feedCreate = feedCreateOpt.IsSet();


                return await Result
                    .Combine(smbShare, smbUser, smbPassword, pathToPackage)
                    .Bind(() => FeedDefinitionSmb.Of(smbShare.Value, smbUser.Value, smbPassword.Value))
                    .Bind((feedDef) => mediator.AddPackageToRemoteFeedAsync(feedDef, pathToPackage.Value, cmdPath, useAbs, feedCreate, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}