
using CSharpFunctionalExtensions;
using Gcd.Handlers.Tools;
using Gcd.Model.Config;
using Gcd.Model.File;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.DownloadNipkg;


namespace Gcd.Commands.Tools;

public static class UseCmdDownloadNipkgExt
{
    private static bool SHOW_IN_HELP = false;
    public static CommandLineApplication UseCmdDownloadNipkg(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(COMMAND, cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.Description = COMMAND_DESCRIPTION;
            var downloadPath = cmd.Option(DOWNLOAD_PATH_OPTION, DOWNLOAD_PATH_DESCRIPTION, CommandOptionType.SingleValue);
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var filePath = LocalFilePath.Offf(downloadPath.Value());
                var installerUri = NipkgInstallerUri.None;

                return await filePath
                    .Map((arg) => new DownloadNipkgRequest(arg, installerUri))
                    .Bind(async (req) => await mediator.Send(req, cancelationToken))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

