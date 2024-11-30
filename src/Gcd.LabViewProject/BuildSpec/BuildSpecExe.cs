using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpecExe : IBuildSpec
{
    private readonly XmlNode _node;

    public static BuildSpecExe Create(XmlNode node)
    {
        return new BuildSpecExe( node);
    }
    private BuildSpecExe(XmlNode node)
    {
        _node = node;
    }

    
    private string MajorVersion => _node.SelectSingleNode("//Property[@Name='Bld_version.major']") is null ? "0" : _node.SelectSingleNode("//Property[@Name='Bld_version.major']").InnerText;
    private string MinorVersion => _node.SelectSingleNode("//Property[@Name='Bld_version.minor']") is null ? "0" : _node.SelectSingleNode("//Property[@Name='Bld_version.minor']").InnerText;
    private string PatchVersion => _node.SelectSingleNode("//Property[@Name='Bld_version.patch']") is null ? "0" : _node.SelectSingleNode("//Property[@Name='Bld_version.patch']").InnerText;
    private string BuildVersion => _node.SelectSingleNode("//Property[@Name='Bld_version.build']") is null ? "0" : _node.SelectSingleNode("//Property[@Name='Bld_version.build']").InnerText;

    public string Name { get => _node.Attributes["Name"]?.Value; }
    public string Type { get => _node.Attributes["Type"]?.Value; }
    public string Target { get => "target"; }
    public string Version { get => $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{BuildVersion}"; }
}