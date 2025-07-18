using Gcd.NiPackageManager;

namespace Gcd.Snapshot.Tests;

public class SnapshotSerializerJsonTests
{
    [Fact]
    public async Task SuccessCase()
    {
        var serializer = new SnapshotSerializerJson();

        
        var testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "snapshot.json");
        string jsonContent = await File.ReadAllTextAsync(testDataPath);
        var snapshot = await serializer.DeserializeAsync(jsonContent);
    }
}