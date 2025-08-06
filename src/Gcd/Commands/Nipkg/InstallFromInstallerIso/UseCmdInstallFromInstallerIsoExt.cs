using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;
using Gcd.Handlers.Nipkg.InstallFromInstallerIso;
using Gcd.LocalFileSystem.Abstractions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.InstallFromInstallerIso;

public static class UseCmdInstallFromInstallerIsoExt
{
    public static string NAME = "install-from-installer-iso";
    public static string DESCRIPTION = "install-from-installer-iso";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdInstallFromInstallerIso(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var isoPathOpt = new IsoLocalFilePath();
            var expandDirectoryOption = new ExpandDirectoryOption();
            var simulateInstallationOpt = new SimulateInstallationOption();
            var packagePatternOpt = new PackagePatternOption();
            var removeIsoFileOpt = new RemoveIsoFileOption();
            var removeExpandDirectoryOpt = new RemoveExpandDirectoryOption();
            
            cmd.AddOptions(
                isoPathOpt.IsRequired(),
                expandDirectoryOption,
                simulateInstallationOpt,
                packagePatternOpt,
                removeIsoFileOpt,
                removeExpandDirectoryOpt
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var pathToIso = isoPathOpt.ToLocalPath();
                var pathToExpandDir = expandDirectoryOption.ToLocalPath();
                var expandDir = Maybe<LocalDirPath>.None;
                if (pathToExpandDir.IsSuccess)
                {
                    expandDir = pathToExpandDir.Value;
                }
                var removeIsoFile = removeIsoFileOpt.HasValue();
                var removeExpandFile = removeExpandDirectoryOpt.HasValue();

                return await Result.Success()
                    .Bind(() => mediator.InstallFromInstallerIsoAsync(pathToIso.Value, expandDir, removeIsoFile,
                        removeExpandFile, Maybe.None, simulateInstallationOpt.HasValue(),  cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}


public sealed class ExpandDirectoryOption : CommandOption
{
    public ExpandDirectoryOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Path to installer directory";
    }
    public static readonly string NAME = "--iso-expand-directory";
    public Result<LocalDirPath> ToLocalPath() =>
        LocalDirPath.Of(this.Value()).MapError(er => er.Message);
}

public sealed class RemoveIsoFileOption : CommandOption
{
    public RemoveIsoFileOption() : base(NAME, CommandOptionType.NoValue)
    {
        Description = "When set, removes iso file after expanding it.";
    }
    public static readonly string NAME = "--remove-iso-file";
}

public sealed class RemoveExpandDirectoryOption : CommandOption
{
    public RemoveExpandDirectoryOption() : base(NAME, CommandOptionType.NoValue)
    {
        Description = "When set, removes expand directory after installation.";
    }
    public static readonly string NAME = "--remove-expand-directory";
}

public sealed class IsoLocalFilePath : CommandOption
{
    public IsoLocalFilePath() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Path to local iso file. File should have *.iso extension.";
    }
    public static readonly string NAME = "--iso-local-path";
    public Result<LocalFilePath> ToLocalPath() =>
        LocalFilePath.Of(this.Value()).MapError(er => er.Message);
}

public sealed class PackagePatternOption : CommandOption
{
    public PackagePatternOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Pattern to match packages for installation";
    }
    public static readonly string NAME = "--package-match-pattern";
}

public sealed class SimulateInstallationOption : CommandOption
{
    public SimulateInstallationOption() : base(NAME, CommandOptionType.NoValue)
    {
        Description = "Simulate installation but makes changes to feeds";
    }
    public static readonly string NAME = "--simulate-installation";
}




