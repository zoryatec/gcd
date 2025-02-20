
using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Tools;
using Gcd.Model.Config;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.Tools;

public static class UseCmdDownloadNipkgExt
{
    public static readonly string NAME = "download-nipkg";
    public static readonly string SUCESS_MESSAGE = "success";
    private static readonly bool  SHOW_IN_HELP = false;
    public static readonly string DESCRIPTION = "Command to download nipkg installer from provided link.";
    public static CommandLineApplication UseCmdDownloadNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.Description = DESCRIPTION;
            var downloadPath = new DownloadNipkgPathOption();
            cmd.AddOptions(downloadPath.IsRequired());

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var filePath = downloadPath.Map();
                var installerUri = NipkgInstallerUri.None;

                 await filePath
                    .Map((arg) => new DownloadNipkgRequest(arg, installerUri))  
                    .Map(async (req) => await mediator.Send(req, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

