using System.Xml;

namespace Gcd.LabViewProject;

public class LabViewBuildSpec
{
    private readonly XmlNode _node;

    public static LabViewBuildSpec Create(XmlNode node)
    {
        return new LabViewBuildSpec( node);
    }
    private LabViewBuildSpec(XmlNode node)
    {
        _node = node;
    }

    public string GetName()
    {
        return _node.Attributes["Name"]?.Value;
    }

    public string GetType()
    {
        return _node.Attributes["Type"]?.Value;
    }
}