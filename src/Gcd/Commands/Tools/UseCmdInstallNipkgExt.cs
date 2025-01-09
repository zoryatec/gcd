
using CSharpFunctionalExtensions;
using Gcd.Commands.Config;
using Gcd.Extensions;
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
    public static readonly bool   SHOW_IN_HELP = false;
    public static readonly string DESCRIPTION = "install-nipkg";
    public static CommandLineApplication UseCmdInstallNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var installerSourceUrlOption = new NipkgInstallerSourceUrlOption();
            cmd.AddOptions(installerSourceUrlOption.IsRequired()); // it is optional untill design finished

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var cmdPath = NipkgCmdPath.None;


                // to be refactores
                var insallerSource = NipkgInstallerUri.None;
                if(installerSourceUrlOption.HasValue())
                {
                   var  instUrl = installerSourceUrlOption.Map();
                    if(instUrl.IsFailure)
                    {
                        console.Error.Write(instUrl.Error);
                        return 1;
                    }
                    else
                    {
                        insallerSource = instUrl.Value;
                    }
                }

                // 1. add optional validation -> consider how to do it flag validate + optional path to nipkg?
                // 2 add installation uri
                return await mediator.InstallNipkgInstallerAsync(insallerSource, cmdPath, cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

