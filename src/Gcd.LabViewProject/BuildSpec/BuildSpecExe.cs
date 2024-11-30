using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpecExe : IBuildSpec
{
    private const string MAJOR_XML_PATH = "//Property[@Name='Bld_version.major']";
    private const string MINOR_XML_PATH = "//Property[@Name='Bld_version.minor']";
    private const string FIX_XML_PATH = "//Property[@Name='Bld_version.patch']";
    private const string BUILD_XML_PATH = "//Property[@Name='Bld_version.build']";

    private readonly XmlNode _node;

    public static BuildSpecExe Create(XmlNode node)
    {
        return new BuildSpecExe( node);
    }

    public void SetVersion(BuildSpecVersion version)
    {
        if(MajorExists) _node.SelectSingleNode(MAJOR_XML_PATH).InnerText = version.Major.ToString();

        if(MinorExists) _node.SelectSingleNode(MINOR_XML_PATH).InnerText = version.Minor.ToString();

        if(FixExists) _node.SelectSingleNode(FIX_XML_PATH).InnerText = version.Fix.ToString();

        if(BuildExists) _node.SelectSingleNode(BUILD_XML_PATH).InnerText = version.Build.ToString();


    }

    private BuildSpecExe(XmlNode node)
    {
        _node = node;
    }

    private bool MajorExists => _node.SelectSingleNode(MAJOR_XML_PATH) is not null;
    private bool MinorExists => _node.SelectSingleNode(MINOR_XML_PATH) is not null;
    private bool FixExists => _node.SelectSingleNode(FIX_XML_PATH) is not null;
    private bool BuildExists => _node.SelectSingleNode(BUILD_XML_PATH) is not null;

    private string MajorVersion => MajorExists ? _node.SelectSingleNode(MAJOR_XML_PATH).InnerText : "0"; 
    private string MinorVersion => MinorExists ? _node.SelectSingleNode(MINOR_XML_PATH).InnerText : "0";
    private string PatchVersion => FixExists ? _node.SelectSingleNode(FIX_XML_PATH).InnerText : "0";
    private string BuildVersion => BuildExists ? _node.SelectSingleNode(BUILD_XML_PATH).InnerText : "0";

    public string Name { get => _node.Attributes["Name"]?.Value; }
    public string Type { get => _node.Attributes["Type"]?.Value; }
    public string Target { get => "target"; }
    public string Version { get => $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{BuildVersion}"; }
}