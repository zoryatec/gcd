using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record PackageCreateRequest(string PackagePath, string PackageName, string PackageVersion, string InstalationDir, string PackageDestinationDir) : IRequest<PackageCreateResponse>;
public record PackageCreateResponse(string result);

public class PackageCreateHandler(IMediator _mediator)
    : IRequestHandler<PackageCreateRequest, PackageCreateResponse>
{
    public async Task<PackageCreateResponse> Handle(PackageCreateRequest request, CancellationToken cancellationToken)
    {

        string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()); 
        string pckgDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());


        if (!Directory.Exists(pckgDirectory))
        {
            // Delete the directory
            Directory.CreateDirectory(pckgDirectory);
        }


        var subRequest = new TemplateCreateRequest(temporaryDirectory, request.PackageName, request.PackageVersion, request.PackageDestinationDir);
        var subResponse = await _mediator.Send(subRequest);


        RunCommand(temporaryDirectory, pckgDirectory);
        string packageFileName = $"{request.PackageName}_{request.PackageVersion}_windows_x64.nipkg";
        string packageFilePath = Path.Combine(pckgDirectory, packageFileName);

        string currentDirectoryPath = Environment.CurrentDirectory;
        string packageDestinationDir = Path.Combine(currentDirectoryPath, request.PackageDestinationDir);

        if (!Directory.Exists(packageDestinationDir))
        {
            // Delete the directory
            Directory.CreateDirectory(packageDestinationDir);
        }


        string packageDestinationFilePath = Path.Combine(packageDestinationDir, packageFileName);
        File.Copy(packageFilePath, packageDestinationFilePath,overwrite: true);

        Directory.Delete(temporaryDirectory, true);

        Directory.Delete(pckgDirectory, true);

        return new PackageCreateResponse("result");
    }

    private void RunCommand(string temporaryDirectory, string pckgDirectory)
    {
        // Initialize the ProcessStartInfo with the command
        string nipkg = @"C:\Program Files\National Instruments\NI Package Manager\nipkg.exe";
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",       // Use "cmd.exe" to run a command
            Arguments = $"/c {nipkg} pack {temporaryDirectory} {pckgDirectory} ", // "/c" tells cmd to run the command and then terminate
            RedirectStandardOutput = true, // Redirect the output of the command
            RedirectStandardError = true,  // Redirect any errors
            UseShellExecute = false,      // Don't use the shell to execute the command
            CreateNoWindow = true        // Don't create a command window
        };

        try
        {
            using (Process process = Process.Start(startInfo))
            {
                // Read the standard output and error
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                // Wait for the command to finish
                process.WaitForExit();

                // Print the output and errors (if any)
                Console.WriteLine("Output:");
                Console.WriteLine(output);

                if (!string.IsNullOrEmpty(errors))
                {
                    Console.WriteLine("Errors:");
                    Console.WriteLine(errors);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running command: {ex.Message}");
        }
    }
}

