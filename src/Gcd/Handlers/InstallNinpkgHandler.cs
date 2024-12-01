

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

        var procesName = RunProgramWithArguments(tempPath, "--quiet --accept-eulas --prevent-reboot");
        WaitForProcessToExit(procesName);


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

    private string RunProgramWithArguments(string programPath, string arguments)
    {

        string procName = "";
        try
        {
            // Initialize ProcessStartInfo to specify the program and arguments
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = programPath,        // Path to the program
                Arguments = arguments,         // Arguments to pass to the program
                UseShellExecute = true,       // Set to false to use the ProcessStartInfo directly
                RedirectStandardOutput = false, // Redirect the output so we can capture it
                RedirectStandardError = false,  // Redirect errors if there are any
                CreateNoWindow = false          // Don't create a new window
            };

            // Start the process
            using (Process process = Process.Start(startInfo))
            {
                // Capture and display output


                process.WaitForExit();  // Wait for the process to exit

                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                procName = process.ProcessName;
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

        return procName;
    }


    static void WaitForProcessToExit(string processName)
    {
        while (true)
        {
            // Check if the process is running
            var process = Process.GetProcessesByName(processName).FirstOrDefault();

            if (process == null)
            {
                // Process has exited
                break;
            }

            // Wait a bit before checking again
            Thread.Sleep(1000); // 1 second delay
        }
    }
}


