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

public static class UseCmdPullMetaExt
{
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly string NAME = "pull-meta-data";
    public static readonly string DESCRIPTION = "pusll-meta-data desc";
    public static CommandLineApplication UseCmdPullMeta(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var feedLocalOpt = new FeedLocalDirOption();
            var smbShareOpt = new SmbShareAddressOption();
            var smbUserOpt = new SmbUserNameOption();
            var smbPasswordOpt = new SmbPasswordOption();


            cmd.AddOptions(
                feedLocalOpt.IsRequired(),
                smbShareOpt.IsRequired(),
                smbUserOpt.IsRequired(),
                smbPasswordOpt.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var smbShare = smbShareOpt.Map();
                var smbUser = smbUserOpt.Map();
                var smbPassword = smbPasswordOpt.Map();
                var localFeedDef = feedLocalOpt.ToLocalFeedDefinition();

                return await Result
                    .Combine(smbShare, smbUser, smbPassword, localFeedDef)
                    .Bind(() => FeedDefinitionSmb.Of(smbShare.Value, smbUser.Value, smbPassword.Value))
                    .Bind((feedDef) => mediator.PullFeedMetaAsync(feedDef, localFeedDef.Value, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}
