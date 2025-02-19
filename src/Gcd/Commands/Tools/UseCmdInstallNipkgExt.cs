
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
    public static readonly bool   SHOW_IN_HELP = true;

    public static readonly string DESCRIPTION = @"Command installing NI Package Manager automatically from provided link.";

    public static readonly string DESCRIPTION_EXTENDED =
        @"
The command download NIPM/NIPKG installer from provided link. 
Adding default link was considered but did not want to breach any license agreement with NI.
The link can be hosted on any web server or storage like azure blob storage but need to be publicly available.
It is possible to workout link from NI when downloading NIPM/NIPKG and use it.
Once the installer is downloaded, it is launch with arguments described here:
https://knowledge.ni.com/KnowledgeArticleDetails?id=kA00Z000000g18jSAA&l
";
    public static CommandLineApplication UseCmdInstallNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText   = SHOW_IN_HELP;
            cmd.Description = DESCRIPTION;
            cmd.ExtendedHelpText = DESCRIPTION_EXTENDED;
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

