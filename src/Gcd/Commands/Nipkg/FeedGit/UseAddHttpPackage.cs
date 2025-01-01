
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
using Gcd.Model.FeedDefinition;
using Gcd.Handlers.Nipkg.Shared;

namespace Gcd.Commands.Nipkg.FeedGit;

public static class UseAddHttpPackageCmdExtensions
{
    public static CommandLineApplication UseAddHttpPackageCmdToGit(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-http-package", cmd =>
        {
            cmd.Description = "add local package to local feed";

            var locPathOpt = new PackageHttpPathOption();
            var gitRepoAddressOpt = new GitRepoAddressOption();
            var gitUserNameOpt = new GitUserNameOption();
            var gitPasswordOpt = new GitPasswordOption();
            var gitCommiterNameOpt = new GitCommitterNameOption();
            var gitCommiterEmailOpt = new GitCommiterEmailOption();
            var gitBranchNameOption = new GitBranchNameOption();
            var useAbsOpt = new UseAbsolutePathOption();
            var feedCreateOpt = new FeedCreateOption();

            cmd.AddOptions(
                locPathOpt.IsRequired(),
                gitRepoAddressOpt.IsRequired(),
                gitUserNameOpt.IsRequired(),
                gitPasswordOpt.IsRequired(),
                gitCommiterNameOpt.IsRequired(),
                gitCommiterEmailOpt.IsRequired(),
                gitBranchNameOption.IsRequired(),
                useAbsOpt,
                feedCreateOpt
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var locPath = locPathOpt.ToPackageHttpPath();
                var gitRepoAddress = gitRepoAddressOpt.Map();
                var gitUserName = gitUserNameOpt.Map();
                var gitPassword = gitPasswordOpt.Map();
                var gitCommiterName = gitCommiterNameOpt.Map();
                var gitCommiterEmail = gitCommiterEmailOpt.Map();
                var giBranchName = gitBranchNameOption.Map();
                var cmdPath = NipkgCmdPath.None;
                var useAbs = useAbsOpt.Map();
                var feedCreate = feedCreateOpt.IsSet();

                return await Result
                    .Combine(locPath, gitRepoAddress, gitUserName, gitPassword, gitCommiterName, gitCommiterEmail, giBranchName)
                    .Bind(() => Result.Success(new FeedDefinitionGit(gitRepoAddress.Value, giBranchName.Value, gitUserName.Value, gitPassword.Value, gitCommiterName.Value, gitCommiterEmail.Value)))
                    .Bind((x) => mediator.AddPackageToRemoteFeedAsync(x, locPath.Value, cmdPath, useAbs, feedCreate, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}





