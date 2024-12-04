using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd
{
    public class TempDirectoryGenerator : ITempDirectoryGenerator
    {
        public string GenerateTempDirectory()
        {
            string temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            if (Directory.Exists(temporaryDirectory))
                Directory.Delete(temporaryDirectory, true);

            Directory.CreateDirectory(temporaryDirectory);
            return temporaryDirectory;
        }
    }
}
