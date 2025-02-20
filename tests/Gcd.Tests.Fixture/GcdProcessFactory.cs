using Microsoft.Extensions.Configuration;

namespace Gcd.Tests.Fixture
{
    public class GcdProcessFactory(IConfiguration configuration) : IGcdProcessFactory
    {
        private readonly string _gcdPath = configuration.GetValue("GcdPath", string.Empty) ?? string.Empty;

        public IGcdProcess Create()
        {
            return new GcdProcess(_gcdPath);
        }
    }
}
