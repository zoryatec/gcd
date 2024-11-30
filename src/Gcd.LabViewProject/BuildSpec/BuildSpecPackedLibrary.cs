using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpecPackedLibrary
{
    private readonly XmlNode _node;

    public static BuildSpecPackedLibrary Create(XmlNode node)
    {
        return new BuildSpecPackedLibrary( node);
    }
    private BuildSpecPackedLibrary(XmlNode node)
    {
        _node = node;
    }

    public string Name { get => _node.Attributes["Name"]?.Value; }
    public string Type { get => _node.Attributes["Type"]?.Value; }
    public string Target { get => "target"; }
    public string Version { get => "version"; }
}