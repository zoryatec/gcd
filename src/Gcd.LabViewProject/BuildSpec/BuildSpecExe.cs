using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpecExe : IBuildSpec
{
    private const string MAJOR_XML_PATH = "Property[@Name='Bld_version.major']";
    private const string MINOR_XML_PATH = "Property[@Name='Bld_version.minor']";
    private const string FIX_XML_PATH = "Property[@Name='Bld_version.patch']";
    private const string BUILD_XML_PATH = "Property[@Name='Bld_version.build']";
    private const string DESTINATION_COUNT_PATH = "Property[@Name='DestinationCount']";

    private readonly XmlAttributeCollection _attributes;
    private readonly XmlNode _node;

    public static BuildSpecExe Create(XmlNode node)
    {
        return new BuildSpecExe( node);
    }

    public void SetVersion(BuildSpecVersion version)
    {
        CreateOrUpdateProperty("Bld_version.major","Int",version.Major.ToString());
        CreateOrUpdateProperty("Bld_version.minor","Int",version.Minor.ToString());
        CreateOrUpdateProperty("Bld_version.patch","Int",version.Fix.ToString());
        CreateOrUpdateProperty("Bld_version.build","Int",version.Build.ToString());
        CreateOrUpdateProperty("Bld_autoIncrement","Bool","false");
    }

    public void SetOutpuDir(string outputDirPath)
    {
        string unixOutputDirPath = outputDirPath.Replace("\\", "/").Replace(":", "");
        unixOutputDirPath = "/" + unixOutputDirPath;
        RemovePropertyIfExists("Bld_localDestDirType");
        CreateOrUpdateProperty("Bld_localDestDir","Path",unixOutputDirPath);
        
        for (int i = 0; i < DestinationCount; i++)
        {
            CreateOrUpdateProperty($"Destination[{i}].path.type","Str","<none>");
            var pathNode = _node.SelectSingleNode($"Property[@Name='Destination[{i}].path']");
            var fileDirName = Path.GetFileName(pathNode.InnerText);
            CreateOrUpdateProperty($"Destination[{i}].path","Path",@$"{unixOutputDirPath}/{fileDirName}");
        }
    }

    private XmlElement CreateProperty(string propertyName, string propertyType,string value)
    {
        // Create a new "Property" element
        XmlElement prop = _node.OwnerDocument.CreateElement("Property");
        prop.SetAttribute("Name", propertyName);
        prop.SetAttribute("Type", propertyType);
        prop.InnerText = value;
        return prop;
    }
    
    private void CreateOrUpdateProperty(string propertyName, string propertyType,string value)
    {
        // Create a new "Property" element
        var property = _node.SelectSingleNode($"Property[@Name='{propertyName}']");
        if (property is null)
        {
            property = CreateProperty(propertyName, propertyType, value);
            _node.AppendChild(property);
        }
        else
        {
            property.InnerText = value;
        }
    }
    
    private void RemovePropertyIfExists(string propertyName)
    {
        var property = _node.SelectSingleNode($"Property[@Name='{propertyName}']");
        if (property is not null)  _node.RemoveChild(property);
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
    // private bool OutputDirExists => _node.SelectSingleNode(OUTPUT_DIR_PATH) is not null;
    private bool DestinationCountExists => _node.SelectSingleNode(DESTINATION_COUNT_PATH) is not null;

    private XmlNode MajorNode => _node.SelectSingleNode(MAJOR_XML_PATH) ?? throw new NullReferenceException(MAJOR_XML_PATH);
    private XmlNode MinorNode => _node.SelectSingleNode(MINOR_XML_PATH) ?? throw new NullReferenceException(MINOR_XML_PATH);
    private XmlNode PatchNode => _node.SelectSingleNode(FIX_XML_PATH) ?? throw new NullReferenceException(FIX_XML_PATH);
    private XmlNode BuildNode => _node.SelectSingleNode(BUILD_XML_PATH) ?? throw new NullReferenceException(BUILD_XML_PATH);
    // private XmlNode OutpudDirPathNode => _node.SelectSingleNode(OUTPUT_DIR_PATH) ?? throw new NullReferenceException(OUTPUT_DIR_PATH);
    private XmlNode DestinationCountNode => _node.SelectSingleNode(DESTINATION_COUNT_PATH) ?? throw new NullReferenceException(DESTINATION_COUNT_PATH);

    private string MajorVersion => MajorExists ? MajorNode.InnerText : "0"; 
    private string MinorVersion => MinorExists ? MinorNode.InnerText : "0";
    private string PatchVersion => FixExists ? PatchNode.InnerText : "0";
    private string BuildVersion => BuildExists ? BuildNode.InnerText : "0";
    
    private int DestinationCount => DestinationCountExists ? int.Parse(DestinationCountNode.InnerText) : 0;

    public string Name { get => _attributes["Name"]?.Value ?? throw new ArgumentNullException("_attributes[\"Name\"]?.Value"); }
    public string Type { get => _attributes["Type"]?.Value ?? throw new ArgumentNullException("_node.Attributes[\"Type\"]?.Value"); }
    public string Target { get => "My Computer"; } // for now but it is wrong
    public string Version { get => $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{BuildVersion}"; }
}