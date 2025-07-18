using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.FeedLocal;
using Gcd.Handlers.Nipkg.InstallFromSnapshot;
using Gcd.Handlers.Nipkg.Shared;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Model.Nipkg.FeedDefinition;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.InstallFromSnapshot;

public static class UseCmdInstallFromSnapshotExt
{
    public static string NAME = "install-from-snapshot";
    public static string DESCRIPTION = "install-from-snapshot";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdInstallFromSnapshot(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var locPathOpt = new SnapshotLocalPathOption();
            
            cmd.AddOptions(
                locPathOpt.IsRequired()
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var pathToSnapshot = locPathOpt.ToLocalPath();

                return await Result.Success()
                    .Bind(() => mediator.InstallFromSnapshotRequest(pathToSnapshot.Value,  cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}


public sealed class SnapshotLocalPathOption : CommandOption
{
    public SnapshotLocalPathOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Path to local snapshot file. File should have *.json extension.";
    }
    public static readonly string NAME = "--snapshot-local-path";
    public Result<LocalFilePath> ToLocalPath() =>
        LocalFilePath.Of(this.Value()).MapError(er => er.Message);
}




