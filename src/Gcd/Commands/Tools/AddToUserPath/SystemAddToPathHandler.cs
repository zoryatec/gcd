using CSharpFunctionalExtensions;
using MediatR;

namespace Gcd.Commands.Tools.AddToUserPath;

public record SystemAddToPathRequest(string PathToAdd, EnvironmentVariableTarget Target) : IRequest<Result>;


public class SystemAddToPathHandler()
    : IRequestHandler<SystemAddToPathRequest, Result>
{
    public async Task<Result> Handle(SystemAddToPathRequest request, CancellationToken cancellationToken)
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

