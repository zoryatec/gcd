using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Model.Config;

namespace Gcd.Commands.Config.SetConfig;

public static class UseSetConfigCmdExtensions
{
    public static CommandLineApplication UseSetConfigCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = new ConfigPropertyFactory();

        app.Command("set-config", command =>
        {
            command.Description = "COMMAND_DESCRIPTION";
            var options = new List<ConfigPropertyOption>
            {
                new NipkgCmdPathOption(),
                new NipkgInstallerUriOption(),
    
            };

            command.AddOptions(options);
            command.OnExecuteAsync(async cancelationToken =>
            {
                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());

                return await properties
                    .Bind((prop) => mediator.SetConfigAsync(prop, cancelationToken))
                    .Tap(() => console.Write("SUCESS_MESSAGE"))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}


public abstract class ConfigPropertyOption(string template, CommandOptionType optionType) : CommandOption(template, optionType)
{
    public abstract Result<ConfigProperty> Map();
}

public class NipkgInstallerUriOption : ConfigPropertyOption
{
    public static readonly string NAME = "--nipkg-installer-uri";
    public NipkgInstallerUriOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public override Result<ConfigProperty> Map() =>
        NipkgInstallerUri.Of(Value())
        .Map(x => x as ConfigProperty);
}

public class NipkgCmdPathOption : ConfigPropertyOption
{
    public static readonly string NAME = "--nipkg-cmd-path";
    public NipkgCmdPathOption() : base(NAME, CommandOptionType.SingleValue)
    {
        Description = "Description";
    }
    public override Result<ConfigProperty> Map() =>
        NipkgCmdPath.Of(Value())
        .Map(x => x as ConfigProperty);
}