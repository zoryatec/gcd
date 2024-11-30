using System.Collections.Immutable;
using System.Xml;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class LabViewProject 
{
    public string Path { get; }
    public string Content { get => _xmlDocument.OuterXml; }
    private readonly List<IBuildSpec> _buildSpecifications;
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

        _buildSpecifications = new List<IBuildSpec>();
        // Extract and print all build specification names
        var buildSpecFactory = new BuildSpecFactory();
        foreach (XmlNode item in buildSpecsNode.SelectNodes("Item"))
        {
            var buildSpec = buildSpecFactory.Create(item);
            _buildSpecifications.Add(buildSpec.Value);
        }
    }

    public List<IBuildSpec> BuildSpecifications
    {
        get => _buildSpecifications;
    }

    public Result<IBuildSpec> GetBuildSpec(string name, string type, string target)
    {
        foreach (var spec in _buildSpecifications)
        { 
            if (spec.Name == name) return Result.Success<IBuildSpec>(spec);
        }
        return Result.Failure<IBuildSpec>("Specification with name not found");
    }

    public Result SetBuildSpecVersion(string name, string type)
    {
        return Result.Success();
    }
}