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

            var pathToAdd = addToUserPath.Option("--path", "Path to be added to user path enviromental variable",CommandOptionType.SingleValue).IsRequired();
            addToUserPath.OnExecute(async () =>
            {
                console.WriteLine(pathToAdd.Value());
                string newPath = pathToAdd.Value(); 

                // Get the current user's PATH environment variable
                string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);

                // Check if the new path is already in the PATH to avoid duplicates
                if (!currentPath.Contains(newPath))
                {
                    // Add the new path to the user's PATH
                    string updatedPath = currentPath + ";" + newPath;

                    // Set the new PATH value for the user (this affects the current user only)
                    Environment.SetEnvironmentVariable("PATH", updatedPath, EnvironmentVariableTarget.User);

                    console.WriteLine($"Added to PATH: {newPath}");
                }
                else
                {
                    console.WriteLine($"Path {newPath} already exists in PATH.");
                }
            });
        });
        return app;
    }
}