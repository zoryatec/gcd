namespace Gcd.Tests.Fixture
{
    public class GcdProcessResponse
    {
        public string Out { get; set; } = string.Empty;
        public string Error{ get; set; } = string.Empty;
        public int Return { get; set; }

    }
    public class GcdProcessRequest
    {
        public string[] Arguments { get; set; } = new string[0];
    }
    public interface IGcdProcess
    {
        public GcdProcessResponse Run(GcdProcessRequest request);
        public GcdProcessResponse Run(string[] request);
    }
}
