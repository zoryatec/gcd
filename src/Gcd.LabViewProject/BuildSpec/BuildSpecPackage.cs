using System.Xml;

namespace Gcd.LabViewProject;

public class BuildSpecPackage : IBuildSpec
{
    private readonly XmlNode _node;
    private readonly XmlAttributeCollection _attributes;
    public static BuildSpecPackage Create(XmlNode node)
    {
        return new BuildSpecPackage( node);
    }
    private BuildSpecPackage(XmlNode node)
    {
        _node = node;
        ArgumentNullException.ThrowIfNull(_node.Attributes, nameof(_node.Attributes));
        _attributes = _node.Attributes;
    }
  public void SetVersion(BuildSpecVersion version)
    {

        CreateOrUpdateProperty("PKG_version","Str",$"{version.Major}.{version.Minor}.{version.Fix}");
        CreateOrUpdateProperty("PKG_buildNumber","Int",version.Build.ToString());
        CreateOrUpdateProperty("NIPKG_addToFeed","Bool","false");
        CreateOrUpdateProperty("PKG_autoIncrementBuild","Bool","false");
    }

    public void SetOutpuDir(string outputDirPath)
    {
        string unixOutputDirPath = outputDirPath.Replace("\\", "/").Replace(":", "");
        unixOutputDirPath = "/" + unixOutputDirPath;
        var packagePath = $"{unixOutputDirPath}/package";
        var installerPath = $"{unixOutputDirPath}/installer";
        
        // package
        RemovePropertyIfExists("PKG_output.Type");
        CreateOrUpdateProperty("PKG_output","Path",packagePath);
        
        //installer
        RemovePropertyIfExists("NIPKG_installerDestination.Type");
        CreateOrUpdateProperty("NIPKG_installerBuiltBefore","Bool","true");
        CreateOrUpdateProperty("NIPKG_installerDestination","Path",installerPath);
        CreateOrUpdateProperty("NIPKG_createInstaller","Bool","true");
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

    public string Name { get => _attributes["Name"]?.Value ?? throw new ArgumentNullException("_attributes[\"Name\"]?.Value"); }
    public string Type { get => _attributes["Type"]?.Value ?? throw new ArgumentNullException("_node.Attributes[\"Type\"]?.Value"); }
    public string Target { get => "My Computer"; } // for now but it is wrong
    public string Version { get => "version"; }
}