using Gcd.Commands.ProjectBuildSpecList;

namespace Gcd.CommandHandlers;

public interface IProjectService
{
    public IReadOnlyCollection<BuildSpecDto> GetBuildSpecList(string projectPath);
}