using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Menu;

public static class UseVersionizeExtensions
{
    public static CommandLineApplication UseVersionizeCmd(this CommandLineApplication app,
        IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command("versionize", versionizeCommand =>
        {
            versionizeCommand.OnExecute(() =>
            {

            });
        });
        return app;
    }
}