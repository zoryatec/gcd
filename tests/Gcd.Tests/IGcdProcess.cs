using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests
{
    public class GcdProcessResponse
    {
        public string Out { get; set; }
        public string Error{ get; set; }
        public int Return { get; set; }

    }
    public class GcdProcessRequest
    {
        public string[] Arguments { get; set; }
    }
    public interface IGcdProcess
    {
        public GcdProcessResponse Run(GcdProcessRequest request);
        public GcdProcessResponse Run(string[] request);
    }
}
