using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record PackageBuilderSetVersionRequest(string PackagePath, string PackageVersion) : IRequest<PackageBuilderSetVersionResponse>;
public record PackageBuilderSetVersionResponse(string result);

public class PackageBuilderSetVersionHandler()
    : IRequestHandler<PackageBuilderSetVersionRequest, PackageBuilderSetVersionResponse>
{
    public async Task<PackageBuilderSetVersionResponse> Handle(PackageBuilderSetVersionRequest request, CancellationToken cancellationToken)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDirectoryPath = Path.Combine(currentDirectoryPath, request.PackagePath);

        string controlDirectoryPath = Path.Combine(packageDirectoryPath, "control");
        string controlFilePath = Path.Combine(controlDirectoryPath, "control");

        string controlFileContent = File.ReadAllText(controlFilePath);

        string newVersion = request.PackageVersion;

        // Regular expression to match the line starting with "Version:"
        string pattern = @"^Version:.*$";
        string replacement = $"Version: {newVersion}";

        // Replace the line
        controlFileContent = Regex.Replace(controlFileContent, pattern, replacement, RegexOptions.Multiline);


        File.WriteAllText(controlFilePath, controlFileContent);

        return new PackageBuilderSetVersionResponse("result");
    }
}


