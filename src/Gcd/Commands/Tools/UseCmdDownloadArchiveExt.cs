
using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Shared;
using Gcd.Handlers.Tools;
using Gcd.Model.Config;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Gcd.Commands.Tools;

public static class UseCmdDownloadArchiveExt
{
    public static readonly string NAME = "download-archive";
    public static readonly string SUCESS_MESSAGE = "success";
    private static readonly bool  SHOW_IN_HELP = true;
    public static readonly string DESCRIPTION = "Command to download archive and extract files.";
    public static CommandLineApplication UseCmdDownloadArchive(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.ShowInHelpText = SHOW_IN_HELP;
            cmd.Description = DESCRIPTION;
            
            // options
            var destinationDirOption = new DownloadArchiveDestinationDirOption();
            var sourceUriOption = new DownloadArchiveSourceUriOption();
            var relativeDirOption = new DownloadArchiveRelativeDirOption();
            cmd.AddOptions(
                destinationDirOption.IsRequired(),
                sourceUriOption.IsRequired(),
                relativeDirOption
                );

            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var dirPath = destinationDirOption.Map();
                var soruceUri = sourceUriOption.Map();
                var relativeDir = relativeDirOption.Map();


                 await  mediator.DownloadArchiveAsync(soruceUri.Value,relativeDir.Value,dirPath.Value, cancelationToken)
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

