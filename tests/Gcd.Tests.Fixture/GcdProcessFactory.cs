using Microsoft.Extensions.Configuration;

namespace Gcd.Tests.Fixture
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
