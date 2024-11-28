using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class LabViewProject
{
    public static Result<LabViewProject> Create(string pathToProject)
    {
        return new LabViewProject();
    }
    
    private LabViewProject()
    {
        
    }
}