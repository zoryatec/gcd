using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Gcd.Services;
using MediatR;

namespace Gcd.Handlers.Tools;

public record SetInifFileParameterRequest(LocalFilePath IniFilePath, string Section, string Key,
    string Value, bool CreateIfNotExists = true) : IRequest<UnitResult<Error>>;

public class SetIniFileParameterHandler()
    : IRequestHandler<SetInifFileParameterRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(SetInifFileParameterRequest request, CancellationToken cancellationToken)
    {
        var (iniFilePath,  section,  key,  createIfNotExists, value) = request;
        
        SetConfigValue(iniFilePath.Value, section, key, createIfNotExists);
        return UnitResult.Success<Error>();
    }
    
    // Content transformation logic for unit testing
    public static List<string> ModifyConfigContent(
        List<string> lines,
        string section,
        string key,
        string value)
    {
        bool sectionFound = false;
        bool keyFound = false;
        var newLines = new List<string>();
        var sectionHeader = $"[{section}]";
        var keyPattern = $"^{Regex.Escape(key)}=";
        var sectionPattern = $"^\\[{Regex.Escape(section)}\\]$";
        var anySectionPattern = @"^\[.+\]$";

        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            if (Regex.IsMatch(line, sectionPattern))
            {
                sectionFound = true;
                newLines.Add(line);
                continue;
            }

            if (sectionFound && Regex.IsMatch(line, keyPattern))
            {
                keyFound = true;
                newLines.Add($"{key}={value}");
                continue;
            }

            if (sectionFound && Regex.IsMatch(line, anySectionPattern) && !Regex.IsMatch(line, sectionPattern))
            {
                if (!keyFound)
                {
                    newLines.Add($"{key}={value}");
                    keyFound = true;
                }
                sectionFound = false;
            }

            newLines.Add(line);
        }

        if (sectionFound && !keyFound)
        {
            newLines.Add($"{key}={value}");
            keyFound = true;
        }

        if (!sectionFound)
        {
            if (newLines.Count > 0 && !string.IsNullOrWhiteSpace(newLines[^1]))
                newLines.Add("");
            newLines.Add(sectionHeader);
            newLines.Add($"{key}={value}");
        }

        return newLines;
    }

    // File I/O wrapper
    public static void SetConfigValue(
        string configPath,
        string section,
        string key,
        string value,
        bool createIfNotExists = false)
    {
        if (string.IsNullOrEmpty(configPath))
            throw new ArgumentNullException(nameof(configPath));
        if (string.IsNullOrEmpty(section))
            throw new ArgumentNullException(nameof(section));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (!File.Exists(configPath))
        {
            if (createIfNotExists)
            {
                var directory = Path.GetDirectoryName(configPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                File.WriteAllText(configPath, "", Encoding.UTF8);
            }
            else
            {
                throw new FileNotFoundException($"Config file not found: {configPath}");
            }
        }

        var lines = File.Exists(configPath)
            ? new List<string>(File.ReadAllLines(configPath, Encoding.UTF8))
            : new List<string>();

        var newLines = ModifyConfigContent(lines, section, key, value);

        File.WriteAllLines(configPath, newLines, Encoding.UTF8);
    }
}


