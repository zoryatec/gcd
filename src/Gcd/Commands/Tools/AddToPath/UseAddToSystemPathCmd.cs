using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CSharpFunctionalExtensions;

namespace Gcd.Commands.Tools.AddToPath;

public static class UseAddToSystemPathCmdExtensions
{
    public static CommandLineApplication UseAddToSystemPathCmd(this CommandLineApplication app,
       IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-to-system-path", addToUserPath =>
        {
            addToUserPath.Description = "adds to user path";

            var pathToAdd = addToUserPath.Option("--path", "Path to be added to user path enviromental variable", CommandOptionType.SingleValue).IsRequired();
            addToUserPath.OnExecuteAsync(async cancelationToken =>
            {
                var maybePath = Maybe.From(pathToAdd.Value);
                if (maybePath.HasValue)
                {
                    var requestToAddPath = new SystemAddToPathRequest(maybePath.Value, EnvironmentVariableTarget.Machine);
                    var response = await mediator.Send(requestToAddPath);

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