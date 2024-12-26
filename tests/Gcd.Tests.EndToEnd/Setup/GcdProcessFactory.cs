using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Tests.EndToEnd.Setup
{
    public class GcdProcessFactory : IGcdProcessFactory
    {
        private readonly string _gcdPath;
        public GcdProcessFactory(IConfiguration configuration )
        {
            _gcdPath = configuration.GetValue("GcdPath", string.Empty);
        }
        public IGcdProcess Create()
        {
            return new GcdProcess(_gcdPath);
        }
    }
}
