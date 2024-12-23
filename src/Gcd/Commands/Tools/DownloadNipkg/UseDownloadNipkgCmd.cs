﻿
using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Gcd.Model.File;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using static Gcd.Contract.Nipkg.DownloadNipkg;


namespace Gcd.Commands.Tools.DownloadNipkg;

public static class UseDownloadNipkgCmdExtensions
{
    public static CommandLineApplication UseDownloadNipkgCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(COMMAND, subCmd =>
        {
            subCmd.Description = COMMAND_DESCRIPTION;
            var downloadPath = subCmd.Option(DOWNLOAD_PATH_OPTION, DOWNLOAD_PATH_DESCRIPTION, CommandOptionType.SingleValue);
            subCmd.OnExecuteAsync(async cancelationToken =>
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

