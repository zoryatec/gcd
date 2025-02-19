using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Extensions;
using Gcd.Model.Nipkg.FeedDefinition;

namespace Gcd.Commands.Nipkg.FeedSmb;

public static class UseCmdAddDirectoryExt
{
    public static string NAME = "add-local-directory";
    public static string DESCRIPTION = "add-local-directory";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdAddDirectory(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var locDirOpt = new PackageLocalDirectoryOption();
            var smbShareOpt = new SmbShareAddressOption();
            var smbUserOpt = new SmbUserNameOption();
            var smbPasswordOpt = new SmbPasswordOption();
            var useAbsOpt = new UseAbsolutePathOption();
            var feedCreateOpt = new FeedCreateOption();

            cmd.AddOptions(
                locDirOpt.IsRequired(),
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
                var pathToDir = locDirOpt.Map();
                var cmdPath = NipkgCmdPath.None;
                var useAbs = useAbsOpt.Map();
                var feedCreate = feedCreateOpt.IsSet();

                // this should be also aserted: pathToDir !!!!!!! refactor
                return await Result
                    .Combine(smbShare, smbUser, smbPassword)
                    .Bind(() => FeedDefinitionSmb.Of(smbShare.Value, smbUser.Value, smbPassword.Value))
                    .Bind((feedDef) => mediator.AddDirToRemoteFeedAsync(feedDef, pathToDir.Value, cmdPath, useAbs, feedCreate, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}
