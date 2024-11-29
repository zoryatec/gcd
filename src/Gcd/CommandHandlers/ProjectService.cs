namespace Gcd.CommandHandlers;

public record BuildSpecDto(string BuildSpecName, string Target, string version);

public class ProjectService : IProjectService
{
    public ProjectService()
    {
        
    }

    public IReadOnlyCollection<BuildSpecDto> GetBuildSpecList(string projectPath)
    {
        return new List<BuildSpecDto>()
        {
            new BuildSpecDto(BuildSpecName: "testNameq", Target: "testTarget", version: "1.0.0")
        };
    }
}