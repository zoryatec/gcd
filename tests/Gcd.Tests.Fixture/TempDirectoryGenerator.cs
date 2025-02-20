namespace Gcd.Tests.Fixture
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
