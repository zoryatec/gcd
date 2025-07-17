using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Nipkg.InstallFromInstallerDirectory;
using Gcd.LocalFileSystem.Abstractions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.InstallFromInstallerDirectory;

public static class UseCmdInstallFromInstallerDirectoryExt
{
    public static string NAME = "install-from-installer-dir";
    public static string DESCRIPTION = "install-from-installer-dir";
    public static string SUCESS_MESSAGE = "success";
    public static CommandLineApplication UseCmdInstallFromInstallerDirectory(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var locPathOpt = new InstallerLocalDirectoryOption();
            var simulateInstallationOpt = new SimulateInstallationOption();
            var packagePatternOpt = new PackagePatternOption();
            
            cmd.AddOptions(
                locPathOpt.IsRequired(),
                simulateInstallationOpt,
                packagePatternOpt
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var pathToSnapshot = locPathOpt.ToLocalPath();

                return await Result.Success()
                    .Bind(() => mediator.InstallFromInstallerDirectoryAsync(pathToSnapshot.Value, packagePatternOpt.Value(),simulateInstallationOpt.HasValue(),  cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });

        return app;
    }
}


public sealed class InstallerLocalDirectoryOption : CommandOption
{
    public InstallerLocalDirectoryOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Path to installer directory";
    }
    public static readonly string NAME = "--installer-local-dir";
    public Result<LocalDirPath> ToLocalPath() =>
        LocalDirPath.Of(this.Value()).MapError(er => er.Message);
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




