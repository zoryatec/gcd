

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;
using Gcd.CommandHandlers;
using Gcd.LabViewProject;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Handlers;

public record InstallNinpkgRequest() : IRequest<InstallNinpkgResponse>;
public record InstallNinpkgResponse(string result);

public class InstallNinpkgHandler(IMediator _mediator)
    : IRequestHandler<InstallNinpkgRequest, InstallNinpkgResponse>
{
    public async Task<InstallNinpkgResponse> Handle(InstallNinpkgRequest request, CancellationToken cancellationToken)
    {
        var nipkgInstaller = "NIPackageManager21.3.0_online.exe";
        var url = $"https://download.ni.com/support/nipkg/products/ni-package-manager/installers/{nipkgInstaller}";
        var tempPath = $@"C:\Projects\{nipkgInstaller}";

        if (File.Exists(tempPath)) File.Delete(tempPath);

        DownloadNipkg(url, tempPath);

        RunProgramWithArguments(tempPath, "--quiet --accept-eulas --prevent-reboot");
        var nipkgPath = @"C:\Program Files\National Instruments\NI Package Manager";
        var requestToAddPath = new SystemAddToUserPathRequest(nipkgPath);
        var response = await _mediator.Send(requestToAddPath);


        return new InstallNinpkgResponse(response.Result);
    }

    private void DownloadNipkg(string fileUrl, string downloadPath)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                // Download the file asynchronously
                client.DownloadFile(fileUrl, downloadPath);
                Console.WriteLine($"File downloaded successfully to {downloadPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
        }
    }

    private void RunProgramWithArguments(string programPath, string arguments)
    {
        try
        {
            // Initialize ProcessStartInfo to specify the program and arguments
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = programPath,        // Path to the program
                Arguments = arguments,         // Arguments to pass to the program
                UseShellExecute = true,       // will run as current user
                RedirectStandardOutput = true, // Redirect the output so we can capture it
                RedirectStandardError = true,  // Redirect errors if there are any
                CreateNoWindow = true          // Don't create a new window
            };

            // Start the process
            using (Process process = Process.Start(startInfo))
            {
                // Capture and display output
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                process.WaitForExit();  // Wait for the process to exit

                // Display the output and errors
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
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}


