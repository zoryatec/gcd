using System.Dynamic;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public record BuildSpecVersion
{
    private readonly int _build;
    public int Major { get; }
    public int Minor { get; }
    public int Fix { get; }

    public static Result<BuildSpecVersion> Create(string version)
    {
        return Result.Success(new BuildSpecVersion(1, 1, 1, 1));
    }
    public static Result<BuildSpecVersion> Create(int major, int minor, int fix, int build)
    {
        return Result.Success(new BuildSpecVersion( major, minor, fix, build));
    }


    
    private BuildSpecVersion(int major, int minor, int fix, int build)
    {
        _build = build;
        Major = major;
        Minor = minor;
        Fix = fix;
    }
}