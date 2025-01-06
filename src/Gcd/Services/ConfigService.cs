using CSharpFunctionalExtensions;
using Gcd.Commands.Config;
using Gcd.LocalFileSystem.Abstractions;
using Gcd.Model.Config;
using Newtonsoft.Json.Linq;

namespace Gcd.Services;

public interface IConfigService
{
    public Task<Result<AppConfig>> GetAppConfigAsync();
    public Task<Result> SetAppconfig(AppConfig config);
    public  Task<Result> SetProperty(ConfigProperty property);

    public Task<Result<ConfigProperty>> GetProperty(ConfigProperty property);

    public Task<Result<IReadOnlyList<ConfigProperty>>> GetProperties(IReadOnlyList<ConfigProperty> properties);
    public  Task<Result> SetProperties(IReadOnlyList<ConfigProperty> properties);
}

public class ConfigService(IFileSystem _fs, SettingsFilePath _path) : IConfigService
{
    public SettingsFilePath Path { get; } = _path;

    public Task<Result<AppConfig>> GetAppConfigAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> SetAppconfig(AppConfig config)
    {
        throw new NotImplementedException();
    }


    public async Task<Result> SetProperty(ConfigProperty property)
    {
        var json = await _fs.ReadTextFileAsync(_path)
                .Map(content => JObject.Parse(content));

        if (json.IsFailure) json = Result.Success(new JObject());

        return await json.Map(json => json[property.GetType().Name] = property.Value.ToString())
        .Bind((key) => _fs.WriteTextFileAsync(Path, json.Value.ToString()));
    }


    public async Task<Result> SetProperties(IReadOnlyList<ConfigProperty> properties)
    {
        var results = new  List<Result>();
        foreach(var property in properties)
        {
            var res = await SetProperty(property);
            results.Add(res);
        }
        return Result.Combine(results.ToArray());
    }

    public async Task<Result<IReadOnlyList<ConfigProperty>>> GetProperties(IReadOnlyList<ConfigProperty> properties)
    {
        var results = new List<Result<ConfigProperty>>();
        foreach (var property in properties)
        {
            var res = await GetProperty(property);
            results.Add(res);
        }
        var result = Result.Combine(results.ToArray());
        if (result.IsFailure) return Result.Failure<IReadOnlyList<ConfigProperty>>(result.Error);

        IReadOnlyList<ConfigProperty> values = results.Select(x => x.Value).ToList();
        return Result.Success(values);
    }



public async Task<Result<ConfigProperty>> GetProperty(ConfigProperty property)
{
    var factory = new ConfigPropertyFactory();
    var json = await _fs.ReadTextFileAsync(_path)
        .Map(content => JObject.Parse(content));

    if (json.IsFailure) return Result.Failure<ConfigProperty>(json.Error);
    var dupa = json.Value ?? throw new NullReferenceException() ;
    string type = property.GetType().Name;

    var stringValue = dupa[type]?.ToString();
    return factory.Create(property, stringValue);

}

}

