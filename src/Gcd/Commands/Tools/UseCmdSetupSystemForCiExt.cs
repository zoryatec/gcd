using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Setup;
using Gcd.Handlers.Tools;
using Gcd.LocalFileSystem.Abstractions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Tools;

public static class UseCmdSetupSystemForCiExt
{
    public static readonly string NAME = "setup-system-for-ci";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = true;
    public static readonly string DESCRIPTION = "setup-system-for-ci";
    
    public static CommandLineApplication UseCmdSetupSystemForCi(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var labViewCliIniFilePathOption = new LabViewCliIniFilePathOption();
            var labViewIniFilePathOption = new LabViewIniFilePathOption();
            cmd.AddOptions(
                labViewCliIniFilePathOption.IsRequired(),
                labViewIniFilePathOption.IsRequired()
                );
    
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var labViewCliIniFilePath = LocalFilePath.Of(labViewCliIniFilePathOption.Value());
                var labViewIniFilePath = LocalFilePath.Of(labViewCliIniFilePathOption.Value());
                
                return await mediator.SetupSystemForCi(labViewCliIniFilePath.Value,labViewIniFilePath.Value, cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

