using CSharpFunctionalExtensions;
using MediatR;

namespace Gcd.Handlers.Tools;

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
                return Result.Success($"Path {newPath} added to {request.Target} PATH.");
            }
            else
            {
                return Result.Success($"Path {newPath} already exists in PATH.");
            }
        }
        return Result.Failure("PATH env variable not found");
    }
}

