using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.FeedDefinition;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.FeedGitHub;

public static class UUseAddLocalPackageCmdExtensions
{
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdAddLocalPackage(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-local-package", cmd =>
        {
            cmd.Description = "add local package to local feed";

            var locPathOpt = new PackageLocalPathOption();
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
                var locPath = locPathOpt.ToPackageLocalPath();
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
                    .Bind(() => Result.Success(new FeedDefinitionGit(gitRepoAddress.Value, giBranchName.Value,gitUserName.Value,gitPassword.Value,gitCommiterName.Value,gitCommiterEmail.Value)))
                    .Bind((x) => mediator.AddPackageToRemoteFeedAsync(x, locPath.Value, cmdPath, useAbs, feedCreate, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}
