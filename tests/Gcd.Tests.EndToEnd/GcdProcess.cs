using CSharpFunctionalExtensions;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class GcdProcess : IGcdProcess
    {
        public GcdProcess()
        {
            
        }
        public GcdProcessResponse Run(GcdProcessRequest request)
        {
            //string command = @"C:\Projects\_Repos\gcd\src\Gcd\bin\Debug\net8.0\Gcd.exe";
            string command = @"D:\a\gcd\gcd\gcd-bin\gcd.exe";

            var arguments = string.Join(" ", request.Arguments);
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process
            {
                StartInfo = processStartInfo
            };

            process.Start();
            process.WaitForExit();

            return new GcdProcessResponse
            {
                Error = process.StandardError.ReadToEnd(),
                Out = process.StandardOutput.ReadToEnd(),
                Return = process.ExitCode
            };
        }

        public GcdProcessResponse Run(string[] request)
        {
            return Run(new GcdProcessRequest() { Arguments = request });
        }
    }
}
