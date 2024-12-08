using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.Commands.NipkgPackageBuild;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgPackageBuilserSetVersion;

public record PackageBuilderSetVersionRequest(PackageDestinationDirectory PackagePath, PackageVersion PackageVersion) : IRequest<Result>;

public class PackageBuilderSetVersionHandler()
    : IRequestHandler<PackageBuilderSetVersionRequest, Result>
{
    public async Task<Result> Handle(PackageBuilderSetVersionRequest request, CancellationToken cancellationToken)
    {
        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDirectoryPath = Path.Combine(currentDirectoryPath, request.PackagePath.Value);

        string controlDirectoryPath = Path.Combine(packageDirectoryPath, "control");
        string controlFilePath = Path.Combine(controlDirectoryPath, "control");

        string controlFileContent = File.ReadAllText(controlFilePath);

        string newVersion = request.PackageVersion.Value;

        // Regular expression to match the line starting with "Version:"
        string pattern = @"^Version:.*$";
        string replacement = $"Version: {newVersion}";

        // Replace the line
        controlFileContent = Regex.Replace(controlFileContent, pattern, replacement, RegexOptions.Multiline);


        File.WriteAllText(controlFilePath, controlFileContent);

        return Result.Success();
    }
}


