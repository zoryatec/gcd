
using CSharpFunctionalExtensions;
using Gcd.Commands.NipkgDownloadNipkg;
using Gcd.Services;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public static class UseDownloadNipkgCmdExtensions
{
    public static CommandLineApplication UseDownloadNipkgCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        const string SUCESS_MESSAGE = "NIPKG download succesfully pushed successully";

        app.Command("download-nipkg", subCmd =>
        {
            subCmd.Description = "Create package template";
            var downloadPath = subCmd.Option("--download-path", "File path must end with exe", CommandOptionType.SingleValue);
            subCmd.OnExecuteAsync(async cancelationToken =>
            {

                var filePath = FilePath.Create(downloadPath.Value());

                return await filePath
                    .Map((arg) => new DownloadNipkgRequest(arg))
                    .Bind(async (req) => await mediator.Send(req))
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });

        });
        return app;
    }
}

