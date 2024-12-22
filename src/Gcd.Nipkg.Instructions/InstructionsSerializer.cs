using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using CSharpFunctionalExtensions;

namespace Gcd.Tests.UnitTest;

public interface IInstructionsSerializer
{
    public Result<string> Serialize(InstructionsFilePackage instrFilePckg);
    public Result<InstructionsFilePackage> Deserialize(string content);
}

public class InstructionsSerializer : IInstructionsSerializer
{
    public Result<InstructionsFilePackage> Deserialize(string content)
    {
        // Set up the XmlSerializer for the Instructions type
        var serializer = new XmlSerializer(typeof(InstructionsFilePackage));

        // Deserialize the XML string
        using (var stringReader = new StringReader(content))
        {
            var obj = (InstructionsFilePackage)serializer.Deserialize(stringReader) ?? throw new NullReferenceException(nameof(InstructionsFilePackage));
            return Result.Success(obj);

        }
    }

    public Result<string> Serialize(InstructionsFilePackage instrFilePckg)
    {
        var serializer = new XmlSerializer(typeof(InstructionsFilePackage));

        // Configure XmlWriterSettings to simplify the output
        var settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true, // Remove <?xml version="1.0"?>
            Indent = true              // Pretty-print the output
        };

        // Create an empty XmlSerializerNamespaces to suppress namespace declarations
        var emptyNamespaces = new XmlSerializerNamespaces();
        emptyNamespaces.Add("", ""); // This clears namespaces like xmlns:p1

        // Serialize the object with the customized settings
        using (var stringWriter = new StringWriter())
        using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
        {
            serializer.Serialize(xmlWriter, instrFilePckg, emptyNamespaces);
            string xml = stringWriter.ToString();
            return xml;
        }
    }
}

