using Azure.Core;
using CSharpFunctionalExtensions;
using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class TempDirectoryProvider : ITempDirectoryProvider
    {
        public async Task<Result<LocalDirPath>> CreateTempDirPathAsync()
        {
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            return LocalDirPath.Parse(path);
        }

        public string GenerateTempDirectory()
        {
            string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            if (Directory.Exists(temporaryDirectory))
                Directory.Delete(temporaryDirectory, true);

            Directory.CreateDirectory(temporaryDirectory);
            return temporaryDirectory;
        }

        public async Task<Result<LocalDirPath>> GenerateTempDirectoryAsync()
        {
            try
            {
                var path = GenerateTempDirectory();
                return LocalDirPath.Parse(path);
            }
            catch (Exception ex) {
                return Result.Failure<LocalDirPath>(ex.Message);
            }
        }
    }
}
