using Gcd.Handlers.Nipkg.Builder;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.Builder;

public static class UseMenuBuilderExt
{
    public static readonly string NAME = "builder";
    public static CommandLineApplication UseMenuBuilder(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command(NAME, cmd =>
        {
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseCmdInit(serviceProvider);
            cmd.UseCmdSetProperty(serviceProvider);
            cmd.UseCmdAddContent(serviceProvider);
            cmd.UseFilePackageInstructionsCmd(serviceProvider);
            cmd.UseCmdPack(serviceProvider);
        });

        return app;
    }
}

