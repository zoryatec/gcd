using System.Dynamic;
using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using Gcd.Common;

namespace Gcd.LabViewProject;

public record BuildSpecVersion
{
    public int Build { get; }
    public int Major { get; }
    public int Minor { get; }
    public int Fix { get; }


    public static Result<BuildSpecVersion> Create(string version)
    {
        string[] parts = version.Split('.');

        // Parse each part as an integer
        int major = int.Parse(parts[0]);
        int minor = int.Parse(parts[1]);
        int fix = int.Parse(parts[2]);
        int build = int.Parse(parts[3]);

        return Result.Success(new BuildSpecVersion(major, minor, fix, build));
    }

    public static Result<BuildSpecVersion,Error> Of(Maybe<string> version)
    {
        return BuildSpecVersion.Create(version.Value).MapError(er => new Error(er));
    }
    public static Result<BuildSpecVersion> Create(int major, int minor, int fix, int build)
    {
        return Result.Success(new BuildSpecVersion( major, minor, fix, build));
    }


  
    private BuildSpecVersion(int major, int minor, int fix, int build)
    {
        Build = build;
        Major = major;
        Minor = minor;
        Fix = fix;
    }
}