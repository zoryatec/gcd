using System.Text.Json;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class LabViewProjectProvider : ILabViewProjectProvider
{
    public Result<LabViewProject> GetProject(string projectPath)
    {
        string projectContent = File.ReadAllText(projectPath);
        var project = LabViewProject.Create(projectContent,projectPath);
        return project;
    }

    public Result Save(LabViewProject project)
    {
        File.WriteAllText(project.Path, project.Content);
        return Result.Success();
    }
}