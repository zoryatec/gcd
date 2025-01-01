using Gcd.Commands.Nipkg.Build;
using Gcd.Commands.Nipkg.Builder.Init;
using Gcd.Commands.Nipkg.Builder.SetProperty;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands.Nipkg.Builder;

public static class UseBuilderExtension
{
    public static CommandLineApplication UseMenuBuilder(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.Command("builder", cmd =>
        {
            cmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                cmd.ShowHelp();
                return 1;
            });
            cmd.UseNipkgPackageBuilderInitmd(serviceProvider);
            cmd.UseNipkgPackageBuilderSetPropertyCmd(serviceProvider);
            cmd.UseAddContentCmd(serviceProvider);
            cmd.UseFilePackageInstructionsCmd(serviceProvider);
            cmd.UsePackCmd(serviceProvider);
        });

        return app;
    }
}

