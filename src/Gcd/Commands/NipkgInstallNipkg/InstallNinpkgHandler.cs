﻿using System.Diagnostics;
using Gcd.Commands.SystemAddToPath;
using MediatR;

namespace Gcd.Commands.NipkgInstallNipkg;

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
        //var tempPath = Path.Combine("C:", "Projects", nipkgInstaller);
        //var stringPath = tempPath.ToString();

        if (File.Exists(tempPath)) File.Delete(tempPath);

        DownloadNipkg(url, tempPath);

        //var procesName = RunProgramWithArguments(tempPath, "--quiet --accept-eulas --prevent-reboot");
        //WaitForProcessToExit(procesName);
        string cmd = $"start /wait {tempPath} --quiet --accept-eulas --prevent-reboot";
        RunCommand(cmd);


        var nipkgPath = @"C:\Program Files\National Instruments\NI Package Manager";
        var requestToAddPath = new SystemAddToPathRequest(nipkgPath, EnvironmentVariableTarget.User);
        var response = await _mediator.Send(requestToAddPath);


        return new InstallNinpkgResponse("");
    }

    private void DownloadNipkg(string fileUrl, string downloadPath)
    {
        /// functionalit does not work replace with download medated
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
            using (Process? process = Process.Start(startInfo))
            {
                // Capture and display output
                _ = process ?? throw new ArgumentNullException(nameof(process));

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

    static void RunCommand(string command)
    {
        // Initialize the ProcessStartInfo with the command and arguments
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",         // Use "cmd.exe" to run a command
            Arguments = command, // "/c" tells cmd to run the command and then terminate
            RedirectStandardOutput = false,  // Redirect the output of the command
            RedirectStandardError = false,   // Redirect any errors
            UseShellExecute = true,       // Don't use the shell to execute the command
            CreateNoWindow = true         // Don't create a command window
        };

        try
        {
            using (Process? process = Process.Start(startInfo))
            {
                _ = process ?? throw new ArgumentNullException(nameof(process));
                // Read the standard output and error


                // Wait for the command to finish
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

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

