using System.Collections.Immutable;
using System.Xml;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class LabViewProject 
{
    public string Path { get; }
    public string Content { get => _xmlDocument.OuterXml; }
    private readonly List<LabViewBuildSpec> _buildSpecifications;
    private List<LabViewBuildSpec> _buildSpecsCopy;
    private XmlDocument _xmlDocument;

    public static Result<LabViewProject> Create(string projectContent, string projectPath)
    {
        return new LabViewProject(projectContent, projectPath);
    }
    
    private LabViewProject(string projectContent, string path)
    {
        Path = path;
        _xmlDocument = new XmlDocument();
        _xmlDocument.LoadXml(projectContent);

        XmlNode buildSpecsNode = _xmlDocument.SelectSingleNode("//Item[@Name='Build Specifications' and @Type='Build']");

        _buildSpecifications = new List<LabViewBuildSpec>();
        // Extract and print all build specification names
        foreach (XmlNode item in buildSpecsNode.SelectNodes("Item"))
        {
            string innerText = item.InnerText;
            var buildSpec = LabViewBuildSpec.Create(item);
            string name = buildSpec.GetName();
            string type = buildSpec.GetType();
            _buildSpecifications.Add(buildSpec);
        }
    }

    public List<LabViewBuildSpec> BuildSpecifications
    {
        get => _buildSpecifications;
    }

    public Result SetBuildSpecVersion(string name, string type)
    {
        return Result.Success();
    }
}