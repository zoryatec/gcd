using System.Xml;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class BuildSpecFactory
{
    public Result<IBuildSpec> Create(XmlNode specNode)
    {
        string type = specNode.Attributes["Type"]?.Value;

        switch (type)
        {
            case "Exe":
                return BuildSpecExe.Create(specNode);
            case "Packed Library":
                return BuildSpecExe.Create(specNode);
            case "{E661DAE2-7517-431F-AC41-30807A3BDA38}":
                return BuildSpecExe.Create(specNode);
            default:
                return BuildSpec.Create(specNode);
        }
    }
}