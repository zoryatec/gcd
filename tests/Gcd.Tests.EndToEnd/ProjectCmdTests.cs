using System.Diagnostics;

namespace Gcd.Tests.EndToEnd;

public class ProjectCmdTests
{
    [Fact]
    public void Test1()
    {
        string command = "dotnet";
        string arguments = "--version";  // Example: get .NET SDK version

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Print the output
        Console.WriteLine($"Output: {output}");
    }
}