using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpec : IBuildSpec
{
    private readonly XmlNode _node;

    public static BuildSpec Create(XmlNode node)
    {
        return new BuildSpec( node);
    }
    private BuildSpec(XmlNode node)
    {
        _node = node;
    }
    
    public string Name { get => _node.Attributes["Name"]?.Value; }
    public string Type { get => _node.Attributes["Type"]?.Value; }
    public string Target { get => "target"; }
    public string Version { get => "version"; }
}