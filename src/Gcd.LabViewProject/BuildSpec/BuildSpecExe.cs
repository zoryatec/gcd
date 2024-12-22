using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpecExe : IBuildSpec
{
    private const string MAJOR_XML_PATH = "//Property[@Name='Bld_version.major']";
    private const string MINOR_XML_PATH = "//Property[@Name='Bld_version.minor']";
    private const string FIX_XML_PATH = "//Property[@Name='Bld_version.patch']";
    private const string BUILD_XML_PATH = "//Property[@Name='Bld_version.build']";

    private readonly XmlAttributeCollection _attributes;
    private readonly XmlNode _node;

    public static BuildSpecExe Create(XmlNode node)
    {
        return new BuildSpecExe( node);
    }

    public void SetVersion(BuildSpecVersion version)
    {
        if(MajorExists) MajorNode.InnerText = version.Major.ToString();

        if(MinorExists) MinorNode.InnerText = version.Minor.ToString();

        if(FixExists) PatchNode.InnerText = version.Fix.ToString();

        if(BuildExists) BuildNode.InnerText = version.Build.ToString();


    }

    private BuildSpecExe(XmlNode node)
    {
        _node = node;
        ArgumentNullException.ThrowIfNull(_node.Attributes, nameof(_node.Attributes));
        _attributes = _node.Attributes;
    }

    private bool MajorExists => _node.SelectSingleNode(MAJOR_XML_PATH) is not null;
    private bool MinorExists => _node.SelectSingleNode(MINOR_XML_PATH) is not null;
    private bool FixExists => _node.SelectSingleNode(FIX_XML_PATH) is not null;
    private bool BuildExists => _node.SelectSingleNode(BUILD_XML_PATH) is not null;

    private XmlNode MajorNode => _node.SelectSingleNode(MAJOR_XML_PATH) ?? throw new NullReferenceException(MAJOR_XML_PATH);
    private XmlNode MinorNode => _node.SelectSingleNode(MINOR_XML_PATH) ?? throw new NullReferenceException(MINOR_XML_PATH);
    private XmlNode PatchNode => _node.SelectSingleNode(FIX_XML_PATH) ?? throw new NullReferenceException(FIX_XML_PATH);
    private XmlNode BuildNode => _node.SelectSingleNode(BUILD_XML_PATH) ?? throw new NullReferenceException(BUILD_XML_PATH);


    private string MajorVersion => MajorExists ? MajorNode.InnerText : "0"; 
    private string MinorVersion => MinorExists ? MinorNode.InnerText : "0";
    private string PatchVersion => FixExists ? PatchNode.InnerText : "0";
    private string BuildVersion => BuildExists ? BuildNode.InnerText : "0";

    public string Name { get => _attributes["Name"]?.Value ?? throw new ArgumentNullException("_attributes[\"Name\"]?.Value"); }
    public string Type { get => _attributes["Type"]?.Value ?? throw new ArgumentNullException("_node.Attributes[\"Type\"]?.Value"); }
    public string Target { get => "target"; }
    public string Version { get => $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{BuildVersion}"; }
}