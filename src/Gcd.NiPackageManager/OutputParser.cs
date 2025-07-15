using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Gcd.NiPackageManager.Abstractions;

namespace Gcd.NiPackageManager;

public class OutputParser
{
    public async Task<Result<InfoInstalledResponse>> ParseInfoInstalledAsync(NiPackageManagerOutput output)
    {
        if (output.ExitCode != 0)  { return Result.Failure<InfoInstalledResponse>(output.Error); }
        

        var lines = output.Output.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None).ToList();
        
        // Assume lines is List<string> containing your output lines
        var packages = new List<List<string>>();
        var currentPackage = new List<string>();
        int blankLineCount = 0;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                blankLineCount++;
                if (blankLineCount >= 3)
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

        var packageDefinitions = new List<PackageDefinition>();
        foreach (var package in parsedPackages)
        {
            var packageDefinition = new PackageDefinition
            (
                package.ContainsKey("Package") ? package["Package"] : string.Empty,
                package.ContainsKey("Version") ? package["Version"] : string.Empty,
                package.ContainsKey("Description") ? package["Description"] : string.Empty,
                package.ContainsKey("Depends") ? package["Depends"] : string.Empty
            );
            packageDefinitions.Add(packageDefinition);
        }
        
        return Result.Success(new InfoInstalledResponse(packageDefinitions.AsReadOnly()));
    }
}