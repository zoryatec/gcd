
using CSharpFunctionalExtensions;
using Gcd.Handlers.Tools;
using Gcd.Model.Config;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Tools;

public static class UseCmdInstallNipkgExt
{
    public static readonly string NAME = "install-nipkg";
    public static readonly string SUCESS_MESSAGE = "success";
    private static readonly bool SHOW_IN_HELP = false;
    public static readonly string DESCRIPTION = "install-nipkg";
    public static CommandLineApplication UseCmdInstallNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, subCmd =>
        {
            subCmd.Description = DESCRIPTION;

            subCmd.OnExecuteAsync(async cancelationToken =>
            {
                var cmdPath = NipkgCmdPath.None;
                return await mediator.InstallNipkgInstallerAsync(cmdPath, cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

