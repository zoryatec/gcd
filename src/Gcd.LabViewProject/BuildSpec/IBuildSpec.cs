namespace Gcd.LabViewProject;

public interface IBuildSpec
{
    public string Name { get; }
    public string Type { get; }
    public string Target { get;}
    public string Version { get;}

    public void SetVersion(BuildSpecVersion version);
    public void SetOutpuDir(string outputDirPath);
}