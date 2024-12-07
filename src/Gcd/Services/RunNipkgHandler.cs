
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
using Gcd.Common;
using Gcd.LabViewProject;
using Gcd.Services;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace Gcd.Commands.NipkgDownloadFeedMetaData;

public record  RunNipkgRequest(string[] arguments) : IRequest<UnitResult<Error>>;

public class  RunNipkgHandler()
    : IRequestHandler< RunNipkgRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle( RunNipkgRequest request, CancellationToken cancellationToken)
    {

        return RunNipkg(request.arguments);
    }


    private UnitResult<Error> RunNipkg(string[] args)
    {
        // Initialize the ProcessStartInfo with the command
        string nipkg = @"""C:\Program Files\National Instruments\NI Package Manager\nipkg.exe""";
        //var tempArgs = new List<string>() { }

        var arguments = string.Join(" ", args);

        //string arguments = $"/c {nipkg} feed-add-pkg {feedDir} {packagePath}";


        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = nipkg,       // Use "cmd.exe" to run a command
            Arguments = arguments, // "/c" tells cmd to run the command and then terminate
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
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            return UnitResult.Failure<Error>(new Error($"Error running command: {ex.Message}"));
        }
    }
}


