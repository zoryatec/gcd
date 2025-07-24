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
    string Value, bool CreateParameterIfNotExists,  bool CreateFileIfNotExists) : IRequest<UnitResult<Error>>;

public class SetIniFileParameterHandler(IMediator mediator)
    : IRequestHandler<SetInifFileParameterRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(SetInifFileParameterRequest request, CancellationToken cancellationToken)
    {
        var (iniFilePath,  section,  key, value, createParameterIfNotExists, createFileIfNotExists) = request;
        return await SetConfigValue(iniFilePath.Value, section, key, value, createParameterIfNotExists,createFileIfNotExists);
    }
    
    public async Task<UnitResult<Error>> SetConfigValue(
        string configPath,
        string section,
        string key,
        string value,
        bool createParameterIfNotExists,
        bool createFileIfNotExists)
    {
        if (string.IsNullOrEmpty(configPath))
            throw new ArgumentNullException(nameof(configPath));
        if (string.IsNullOrEmpty(section))
            throw new ArgumentNullException(nameof(section));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        if (!File.Exists(configPath))
        {
            if (createFileIfNotExists)
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

        var result = await mediator.Send(new SetIniParameterRequest(lines, section, key, value, createParameterIfNotExists));
        if (result.IsFailure)
        {
            return UnitResult.Failure<Error>(new Error(result.Error));
        }

        await File.WriteAllLinesAsync(configPath, result.Value, Encoding.UTF8);
        return UnitResult.Success<Error>();
    }
}


