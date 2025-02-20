using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpecPackedLibrary : IBuildSpec
{
    private readonly XmlNode _node;
    private readonly XmlAttributeCollection _attributes;
    public static BuildSpecPackedLibrary Create(XmlNode node)
    {
        return new BuildSpecPackedLibrary( node);
    }
    private BuildSpecPackedLibrary(XmlNode node)
    {
        _node = node;
        ArgumentNullException.ThrowIfNull(_node.Attributes, nameof(_node.Attributes));
        _attributes = _node.Attributes;
    }
    public void SetVersion(BuildSpecVersion version)
    {
        throw new NotImplementedException();
    }

    public void SetOutpuDir(string outputDirPath)
    {
        throw new NotImplementedException();
    }

    public string Name { get => _attributes["Name"]?.Value ?? throw new ArgumentNullException("_attributes[\"Name\"]?.Value"); }
    public string Type { get => _attributes["Type"]?.Value ?? throw new ArgumentNullException("_node.Attributes[\"Type\"]?.Value"); }
    public string Target { get => "target"; }
    public string Version { get => "version"; }
}