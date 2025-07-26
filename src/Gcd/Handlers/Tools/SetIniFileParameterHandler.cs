using System.Text;
using CSharpFunctionalExtensions;
using Gcd.Common;
using Gcd.LocalFileSystem.Abstractions;
using MediatR;

namespace Gcd.Handlers.Tools;

public record IniParmeterKey(string Value);
public record IniParmeterValue(string Value);
public record IniParameterSection(string Value);

public record SetInifFileParameterRequest(ILocalFilePath IniFilePath, string Section, string Key,
    string Value, bool CreateParameterIfNotExists,  bool CreateFileIfNotExists) : IRequest<UnitResult<Error>>;

public class SetIniFileParameterHandler(IMediator mediator, IFileSystem fileSystem)
    : IRequestHandler<SetInifFileParameterRequest, UnitResult<Error>>
{
    public async Task<UnitResult<Error>> Handle(SetInifFileParameterRequest request, CancellationToken cancellationToken)
    {
        var (iniFilePath,  section,  key, value, createParameterIfNotExists, createFileIfNotExists) = request;
        return await SetConfigValue(iniFilePath, section, key, value, createParameterIfNotExists,createFileIfNotExists);
    }
    
    public async Task<UnitResult<Error>> SetConfigValue(
        ILocalFilePath configPath,
        string section,
        string key,
        string value,
        bool createParameterIfNotExists,
        bool createFileIfNotExists)
    {
        if (string.IsNullOrEmpty(configPath.Value))
            throw new ArgumentNullException(nameof(configPath));
        if (string.IsNullOrEmpty(section))
            throw new ArgumentNullException(nameof(section));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        // fileSystem.FileExists(configPath);
        if (!File.Exists(configPath.Value))
        {
            if (createFileIfNotExists)
            {
                var directory = Path.GetDirectoryName(configPath.Value);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                File.WriteAllText(configPath.Value, "", Encoding.UTF8);
            }
            else
            {
                throw new FileNotFoundException($"Config file not found: {configPath}");
            }
        }

        var lines = File.Exists(configPath.Value)
            ? new List<string>(File.ReadAllLines(configPath.Value, Encoding.UTF8))
            : new List<string>();

        var result = await mediator.Send(new SetIniParameterRequest(lines, section, key, value, createParameterIfNotExists))
            .Bind(content => fileSystem.WriteAllLinesAsync(configPath, content, Encoding.UTF8));
        
        return UnitResult.Success<Error>();
    }
}


