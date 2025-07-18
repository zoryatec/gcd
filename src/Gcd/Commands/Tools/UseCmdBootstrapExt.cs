using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.Extensions;
using Gcd.Handlers.Tools;
using Gcd.Model.Config;

namespace Gcd.Commands.Tools;

public static class UseCmdBootstrapExt
{
    private static bool SHOW_IN_HELP = true;
    private static string DESCRIPTION = "Command to add provided path to System PATH variable. It requires administrator privileges.";
    public static CommandLineApplication UseCmdBootstrap(this CommandLineApplication app,
       IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("bootstrap", cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.ShowInHelpText = SHOW_IN_HELP;

            var installerSourceUrlOption = new NipkgInstallerSourceUrlOption();
            cmd.AddOptions(installerSourceUrlOption.IsRequired());

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                return await installerSourceUrlOption.Map()
                    .Bind(x => mediator.BootstrapAsync(x, cancelationToken))
                    .Tap(() => console.Write("Path added sucessfully"))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
    
    private sealed class NipkgInstallerSourceUrlOption : CommandOption
    {
        public static readonly string NAME = "--nipkg-installer-source-uri";
        public static readonly string DESCRIPTION = "The URL of the NIPM/NIPKG installer.";
        public NipkgInstallerSourceUrlOption() : base (NAME, CommandOptionType.SingleValue)
        {
            Description = DESCRIPTION;
        }

        public Result<NipkgInstallerUri> Map() =>
            NipkgInstallerUri.Of(this.Value());
    }
}