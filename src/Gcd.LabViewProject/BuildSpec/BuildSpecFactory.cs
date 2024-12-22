using System.Xml;
using System.Xml.Linq;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class BuildSpecFactory
{
    public Result<IBuildSpec> Create(XmlNode specNode)
    {
        ArgumentNullException.ThrowIfNull(specNode.Attributes, nameof(specNode.Attributes));
        var attributes = specNode.Attributes;

        string type = attributes["Type"]?.Value ?? throw new ArgumentNullException("attributes[\"Type\"]?.Value");

        switch (type)
        {
            case "EXE":
                return BuildSpecExe.Create(specNode);
            case "Packed Library":
                return BuildSpecPackedLibrary.Create(specNode);
            case "{E661DAE2-7517-431F-AC41-30807A3BDA38}":
                return BuildSpecPackage.Create(specNode);
            default:
                return BuildSpec.Create(specNode);
        }
    }
}