using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Gcd.NiPackageManager.Abstractions;

namespace Gcd.Providers;

internal class InternalOutputParserToBeRefactored
{
    
    public Result<List<Dictionary<string, string>>> ParsePackagesIntoDictionary(string content)
    {
        // Assume lines is List<string> containing your output lines
        int breakLineCount = 1;
        var packages = new List<List<string>>();
        var currentPackage = new List<string>();
        int blankLineCount = 0;
        var lines = content.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None).ToList();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                blankLineCount++;
                if (blankLineCount >= breakLineCount)
                {
                    if (currentPackage.Count > 0)
                    {
                        packages.Add(new List<string>(currentPackage));
                        currentPackage.Clear();
                    }
                    blankLineCount = 0;
                }
            }
            else
            {
                blankLineCount = 0;
                currentPackage.Add(line);
            }
        }

        if (currentPackage.Count > 0)
        {
            packages.Add(new List<string>(currentPackage));
        }


        var parsedPackages = new List<Dictionary<string, string>>();

        foreach (var packageLines in packages)
        {
            var packageProps = new Dictionary<string, string>();
            foreach (var line in packageLines)
            {
                var match = Regex.Match(line, @"^(.*?):\s*(.*)$");
                if (match.Success)
                {
                    var key = match.Groups[1].Value;
                    var value = match.Groups[2].Value;
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        packageProps[key] = value;
                    }
                }
            }
            if (packageProps.Count > 0)
            {
                parsedPackages.Add(packageProps);
            }
        }

        return Result.Success(parsedPackages);
    }
    
}