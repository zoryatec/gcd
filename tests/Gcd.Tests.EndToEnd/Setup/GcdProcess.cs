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
        public GcdProcess(string gcdPath)
        {
            GcdPath = gcdPath;
        }

        private readonly string GcdPath;

        public GcdProcessResponse Run(GcdProcessRequest request)
        {
            var arugmentss = new List<string>();
            foreach(var arg in request.Arguments) // envelop args with quotation mark
            {
                arugmentss.Add($"\"{arg}\"");
            }

            var arguments = string.Join(" ", arugmentss);
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = GcdPath,
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
