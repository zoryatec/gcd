using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public interface ILabViewProjectProvider
{
    Result<LabViewProject> GetProject(string projectPath);
    Result Save(LabViewProject project);
}