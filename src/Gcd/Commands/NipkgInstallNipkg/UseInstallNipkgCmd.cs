
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadNipkg;
using Gcd.Model;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.InstallNipkg;


namespace Gcd.Commands.NipkgInstallNipkg;

public static class UseInstallNipkgCmdExtensions
{
    public static CommandLineApplication UseInstallNipkgCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(COMMAND, subCmd =>
        {
            subCmd.Description = COMMAND_DESCRIPTION;

            subCmd.OnExecuteAsync(async cancelationToken =>
            {

                return  await mediator.InstallNipkgInstallerAsync(cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

