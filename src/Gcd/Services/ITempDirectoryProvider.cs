using CSharpFunctionalExtensions;
using Gcd.Model.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public interface ITempDirectoryProvider
    {
        public string GenerateTempDirectory();

        public Task<Result<LocalDirPath>> GenerateTempDirectoryAsync();

        public Task<Result<LocalDirPath>> CreateTempDirPathAsync();
    }
}
