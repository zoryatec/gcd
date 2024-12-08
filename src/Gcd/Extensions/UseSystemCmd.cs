using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Gcd.CommandHandlers;
using Gcd.Handlers;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Gcd.Extensions;

public static class UseSystemExtensions
{
    public static CommandLineApplication UseSystemCmd(this CommandLineApplication app, IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();

        app.Command("system", systemCmd =>
        {
            systemCmd.OnExecute(() =>
            {
                console.WriteLine("Specify a subcommand");
                systemCmd.ShowHelp();
                return 1;
            });
            systemCmd.UseAddToUserPathCmd(serviceProvider);
            systemCmd.UseAddToSystemPathCmd(serviceProvider);
        });
        return app;
    }

    public static CommandLineApplication UseAddToUserPathCmd(this CommandLineApplication app,
    IServiceProvider serviceProvider)
    {
        var console = serviceProvider.GetRequiredService<IConsole>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        app.Command("add-to-user-path", addToUserPath =>
        {
            addToUserPath.Description = "adds to user path";

            var pathToAdd = addToUserPath.Argument("path", "Path to be added to user path enviromental variable").IsRequired();

            addToUserPath.OnExecuteAsync( async cancelationToken =>
            {
                var maybePath = Maybe.From(pathToAdd.Value);
                if(maybePath.HasValue)
                {
                    var requestToAddPath = new SystemAddToPathRequest(maybePath.Value, EnvironmentVariableTarget.User);
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