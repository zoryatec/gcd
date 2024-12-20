using CSharpFunctionalExtensions;
using Gcd.Commands.Config.SetConfig;
using Gcd.Model.Config;
using MediatR;

namespace Gcd.Commands.Tools.AddToPath;

public record AddToPathRequest(string PathToAdd, EnvironmentVariableTarget Target) : IRequest<Result>;


public class AddToPathHandler()
    : IRequestHandler<AddToPathRequest, Result>
{
    public async Task<Result> Handle(AddToPathRequest request, CancellationToken cancellationToken)
    {
        string newPath = request.PathToAdd;
        var currentPath = Maybe.From(Environment.GetEnvironmentVariable("PATH", request.Target));

        if (currentPath.HasValue)
        {
            if (!currentPath.Value.Contains(newPath))
            {
                string updatedPath = currentPath + ";" + newPath;
                Environment.SetEnvironmentVariable("PATH", updatedPath, request.Target);
                return Result.Success();
            }
            else
            {
                return Result.Failure($"Path {newPath} already exists in PATH.");
            }
        }
        return Result.Failure("PATH env variable not found");
    }
}

public static class MediatorExtensions
{
    public static async Task<Result> AddToUserPath(this IMediator mediator, string pathToAdd, CancellationToken cancellationToken = default)
        => await mediator.Send(new AddToPathRequest(pathToAdd,EnvironmentVariableTarget.User), cancellationToken);
    public static async Task<Result> AddToSystemPath(this IMediator mediator, string pathToAdd, CancellationToken cancellationToken = default)
        => await mediator.Send(new AddToPathRequest(pathToAdd, EnvironmentVariableTarget.Machine), cancellationToken);
}
