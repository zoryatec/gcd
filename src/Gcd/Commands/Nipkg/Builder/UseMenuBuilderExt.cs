using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.Builder;

public static class UseMenuBuilderExt
{
    public static readonly string NAME = "builder";
    public const string DESCRIPTION = "Commands for performing package creation process step by step, offloading the burden of manually creating the entire structure.";

    public const string DESCRIPTION_EXTENDED = @"
The primary purpose of these commands is to overcome limitations of built command.
";
    public static CommandLineApplication UseMenuBuilder(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command(NAME, cmd =>
        {
            cmd.Description = DESCRIPTION;
            cmd.ExtendedHelpText = DESCRIPTION_EXTENDED;
            cmd.OnExecute(() =>
            {
                console.WriteLine("");
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

