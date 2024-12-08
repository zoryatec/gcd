using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record SystemAddToPathRequest(string PathToAdd, EnvironmentVariableTarget Target) : IRequest<Result>;


public class SystemAddToPathHandler()
    : IRequestHandler<SystemAddToPathRequest, Result>
{
    public async Task<Result> Handle(SystemAddToPathRequest request, CancellationToken cancellationToken)
    {
        string newPath = request.PathToAdd;

        // Get the current user's PATH environment variable
        //string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
        var currentPath = Maybe.From(Environment.GetEnvironmentVariable("PATH", request.Target));

        if(currentPath.HasValue)
        {
            // Check if the new path is already in the PATH to avoid duplicates
            if (!currentPath.Value.Contains(newPath))
            {
                // Add the new path to the user's PATH
                string updatedPath = currentPath + ";" + newPath;

                // Set the new PATH value for the user (this affects the current user only)
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

