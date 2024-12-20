using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;

namespace Gcd.Commands.Tools.AddToPath;

public static class UseAddToUserPathCmdExtensions
{
    public static CommandLineApplication UseAddToUserPathCmd(this CommandLineApplication app,
    IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-to-user-path", addToUserPath =>
        {
            addToUserPath.Description = "adds to user path";

            var pathToAdd = addToUserPath.Argument("path", "Path to be added to user path enviromental variable").IsRequired();

            addToUserPath.OnExecuteAsync(async cancelationToken =>
            {
                var maybePath = Maybe.From(pathToAdd.Value);
                if (maybePath.HasValue)
                {
                    var response = await mediator.AddToUserPath(maybePath.Value);

                    return response
                        .Tap(() => console.Write("Path added sucessfully"))
                        .TapError(error => console.Error.Write(error))
                        .Finally(x => x.IsFailure ? 1 : 0);

                }
                console.Error.Write("Path not given");
                return 1;
            });
        });
        return app;
    }
}