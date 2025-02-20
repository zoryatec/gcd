using System.Text;
using System.Text.Json;
using System.Xml;
using CSharpFunctionalExtensions;

namespace Gcd.LabViewProject;

public class LabViewProjectProvider : ILabViewProjectProvider
{
    public Result<LabViewProject> GetProject(string projectPath)
    {
        string projectContent = File.ReadAllText(projectPath);
        var project = LabViewProject.Create(projectContent,projectPath);
        return project;
    }

    public Result Save(LabViewProject project)
    {
        // Set up the XmlWriterSettings
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,                          // Enable indentation
            NewLineChars = "\r\n",                  // Set new line characters
            Encoding = new UTF8Encoding(true)      // Set UTF-8 encoding with BOM
        };

        // Create an XmlWriter with the specified settings
        using (XmlWriter writer = XmlWriter.Create(project.Path, settings))
        {
            // Write the XML document using the XmlWriter
            project.XmlDocument.Save(writer);
        }
        return Result.Success();
    }
}