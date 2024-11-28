namespace Gcd.CommandHandlers;

public interface IProjectService
{
    public IReadOnlyCollection<BuildSpecDto> GetBuildSpecList(string projectPath);
}