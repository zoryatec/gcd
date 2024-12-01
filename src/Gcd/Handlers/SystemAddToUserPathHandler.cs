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

public record SystemAddToUserPathRequest(string PathToAdd) : IRequest<SystemAddToUserPathResponse>;
public record SystemAddToUserPathResponse(string Result);

public class SystemAddToUserPathHandler()
    : IRequestHandler<SystemAddToUserPathRequest, SystemAddToUserPathResponse>
{
    public async Task<SystemAddToUserPathResponse> Handle(SystemAddToUserPathRequest request, CancellationToken cancellationToken)
    {
        string newPath = request.PathToAdd;
        string result = "";

        // Get the current user's PATH environment variable
        string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);

        // Check if the new path is already in the PATH to avoid duplicates
        if (!currentPath.Contains(newPath))
        {
            // Add the new path to the user's PATH
            string updatedPath = currentPath + ";" + newPath;

            // Set the new PATH value for the user (this affects the current user only)
            Environment.SetEnvironmentVariable("PATH", updatedPath, EnvironmentVariableTarget.User);

            result = $"Added to PATH: {newPath}";
        }
        else
        {
            result = $"Path {newPath} already exists in PATH.";
        }

        return new SystemAddToUserPathResponse(result);
    }
}

