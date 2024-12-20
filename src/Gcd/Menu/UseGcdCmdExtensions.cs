using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Menu;

public static class UseGcdCmdExtensions
{
    public static CommandLineApplication UseGcdCmd(this CommandLineApplication app,
        IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        app.VersionOption("-v|--version", GetVersion());

        app.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(serviceProvider);

        app.HelpOption(inherited: true);

        app.UseProjectCmd(serviceProvider);
        app.UseNipkgCmd(serviceProvider);
        app.UseToolsCmd(serviceProvider);
        app.UseConfigCmd(serviceProvider);


        app.OnExecute(() =>
        {
            console.WriteLine("Specify a subcommand");
            app.ShowHelp();
            return 1;
        });

        return app;
    }
    private static string GetVersion() => typeof(Program).Assembly.GetName().Version?.ToString() ?? "";
}