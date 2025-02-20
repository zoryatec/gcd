using Gcd.Commands.Nipkg;
using Gcd.Commands.Project;
using Gcd.Commands.Tools;
using Gcd.Commands.Vipm;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Commands;

public static class UseMenuGcdExt
{
    public static CommandLineApplication UseMenuGcd(this CommandLineApplication app,
        IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.VersionOption("-v|--version", GetVersion());

        app.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(serviceProvider);

        //app.HelpOption(inherited: true);

        app.UseLabViewCmd(serviceProvider);
        app.UseMenuNipkg(serviceProvider);
        app.UseMenuTools(serviceProvider);
        app.UseMenuConfig(serviceProvider);
        app.UseMenuVipm(serviceProvider);


        app.OnExecute(() =>
        {
            app.ShowHelp();
            return 1;
        });

        return app;
    }
    private static string GetVersion() => typeof(Program).Assembly.GetName().Version?.ToString() ?? "";
}