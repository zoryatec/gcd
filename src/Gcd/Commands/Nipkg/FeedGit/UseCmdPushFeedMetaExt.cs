using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Model.Config;
using Gcd.Extensions;
using Gcd.Model.FeedDefinition;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.Nipkg.FeedDefinition;

namespace Gcd.Commands.Nipkg.FeedGit;

public static class UseCmdPushFeedMetaExt
{
    public static string SUCESS_MESSAGE = "success";
    public static string NAME = "push-meta-data";
    public static CommandLineApplication UseCmdPushFeedMeta(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = "add local package to local feed";

            var feedLocalOpt = new FeedLocalDirOption();
            var gitRepoAddressOpt = new GitRepoAddressOption();
            var gitUserNameOpt = new GitUserNameOption();
            var gitPasswordOpt = new GitPasswordOption();
            var gitCommiterNameOpt = new GitCommitterNameOption();
            var gitCommiterEmailOpt = new GitCommiterEmailOption();
            var gitBranchNameOption = new GitBranchNameOption();


            cmd.AddOptions(
                feedLocalOpt.IsRequired(),
                gitRepoAddressOpt.IsRequired(),
                gitUserNameOpt.IsRequired(),
                gitPasswordOpt.IsRequired(),
                gitCommiterNameOpt.IsRequired(),
                gitCommiterEmailOpt.IsRequired(),
                gitBranchNameOption.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var localFeedDef = feedLocalOpt.ToLocalFeedDefinition();
                var gitRepoAddress = gitRepoAddressOpt.Map();
                var gitUserName = gitUserNameOpt.Map();
                var gitPassword = gitPasswordOpt.Map();
                var gitCommiterName = gitCommiterNameOpt.Map();
                var gitCommiterEmail = gitCommiterEmailOpt.Map();
                var giBranchName = gitBranchNameOption.Map();
                var cmdPath = NipkgCmdPath.None;


                return await Result
                    .Combine(gitRepoAddress, gitUserName, gitPassword, gitCommiterName, gitCommiterEmail, giBranchName, localFeedDef)
                    .Bind(() => Result.Success(new FeedDefinitionGit(gitRepoAddress.Value, giBranchName.Value, gitUserName.Value, gitPassword.Value, gitCommiterName.Value, gitCommiterEmail.Value)))
                    .Bind((x) => mediator.PushFeedMetaDataAsync(x, localFeedDef.Value, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}
