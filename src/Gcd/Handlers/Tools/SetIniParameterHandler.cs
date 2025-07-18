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

public record SetIniParameterRequest(List<string> lines, string Section, string Key,
    string Value, bool CreateIfNotExists = true) : IRequest<Result<List<string>>>;

public class SetIniParameterHandler()
    : IRequestHandler<SetIniParameterRequest, Result<List<string>>>
{
    public async Task<Result<List<string>>> Handle(SetIniParameterRequest request, CancellationToken cancellationToken)
    {
        var (lines,  section,  key,  createIfNotExists, value) = request;
        
       var result = ModifyConfigContent(lines, section, key, createIfNotExists);
       return Result.Success(result);
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
}


