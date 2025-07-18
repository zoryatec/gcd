using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Handlers.Setup;
using Gcd.Handlers.Tools;
using Gcd.LocalFileSystem.Abstractions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Tools;

public static class UseCmdSetIniFileParameterExt
{
    public static readonly string NAME = "set-ini-parameter";
    public static readonly string SUCESS_MESSAGE = "success";
    public static readonly bool   SHOW_IN_HELP = true;
    public static readonly string DESCRIPTION = "set-ini-parameter";
    
    public static CommandLineApplication UseCmdSetIniFileParameter(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            var iniFilePathOption = new IniFilePathOption();
            var iniFileSectionOption = new IniFileSectionOption();
            var iniFileKeOption = new InifFileKeyOption();
            var iniFileValueOption = new InifFileValueOption();
            cmd.AddOptions(
                iniFilePathOption.IsRequired(),
                iniFileSectionOption.IsRequired(),
                iniFileKeOption.IsRequired(),
                iniFileValueOption.IsRequired()
                );
    
            cmd.OnExecuteAsync(async cancelationToken =>
            {
                var iniFilePath = LocalFilePath.Of(iniFilePathOption.Value());
                var section = iniFileSectionOption.Value()?? throw new ArgumentException("Section cannot be null or empty");
                var iniFileKe = iniFileKeOption.Value() ?? throw new ArgumentException("Key cannot be null or empty");
                var iniFileValue = iniFileValueOption.Value() ?? throw new ArgumentException("Value cannot be null or empty");
                
                return await mediator.SetIniFileParameteAsync(iniFilePath.Value,section,
                        iniFileKe, iniFileValue, true, cancelationToken)
                    .Tap(() => console.Write(SUCESS_MESSAGE))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}

