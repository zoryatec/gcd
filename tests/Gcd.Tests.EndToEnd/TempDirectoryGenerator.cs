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
            throw new NotImplementedException();
        }
    }
}
