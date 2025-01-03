using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;
using Gcd.Extensions;
using Gcd.Model.Config;
using Gcd.Handlers.Config;

namespace Gcd.Commands.Config;

public static class UseCmdGetExt
{
    public static CommandLineApplication UseCmdGet(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var factory = new ConfigPropertyFactory();

        app.Command("get", command =>
        {
            command.Description = "COMMAND_DESCRIPTION";
            var options = new List<ConfigPropertyOption>
            {
                new GetNipkgInstallerUriOption(),
                new GetNipkgCmdPathOption(),

            };

            command.AddOptions(options);
            command.OnExecuteAsync(async cancelationToken =>
            {
                var properties = factory.Create(options.Where(x => x.HasValue()).ToList());

                return await properties
                    .Bind((prop) => mediator.GetConfigAsync(prop, cancelationToken))
                    .Tap((x) => console.Write(string.Join("\n", x.Select(x => x.Value))))
                    .TapError(error => console.Error.Write(error))
                    .Finally(x => x.IsFailure ? 1 : 0);
            });
        });
        return app;
    }
}


public class GetNipkgInstallerUriOption : ConfigPropertyOption
{
    public static readonly string NAME = "--nipkg-installer-uri";
    public GetNipkgInstallerUriOption(CommandOptionType optionType = CommandOptionType.NoValue) : base(NAME, optionType)
    {
        Description = "Description";
    }
    public override Result<ConfigProperty> Map() =>
        Result.Success(NipkgInstallerUri.None as ConfigProperty);
}

public class GetNipkgCmdPathOption : ConfigPropertyOption
{
    public static readonly string NAME = "--nipkg-cmd-path";
    public GetNipkgCmdPathOption(CommandOptionType optionType = CommandOptionType.NoValue) : base(NAME, optionType)
    {
        Description = "Description";
    }
    public override Result<ConfigProperty> Map() =>
        Result.Success(NipkgCmdPath.None as ConfigProperty);
}