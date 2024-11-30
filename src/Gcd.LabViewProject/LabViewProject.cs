using System.Collections.Immutable;
using System.Xml;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class LabViewProject
{
    private string _fileContent;
    private readonly List<LabViewBuildSpec> _buildSpecifications;
    private List<LabViewBuildSpec> _buildSpecsCopy;

    public static Result<LabViewProject> Create(string fileContent)
    {
        return new LabViewProject(fileContent);
    }
    
    private LabViewProject(string fileContent)
    {
        _fileContent = fileContent;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(_fileContent);

        XmlNode buildSpecsNode = doc.SelectSingleNode("//Item[@Name='Build Specifications' and @Type='Build']");

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
}