using Gcd.CommandHandlers;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Extensions;

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
                var handler = serviceProvider.GetRequiredService<IVersionizeCommandHandler>();
                handler.Handle();
            });
        });
        return app;
    }
}