using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services;

    public interface IConfigService
    {
        public Task<Result<AppConfig>> GetAppConfigAsync();
        public Task<Result> SetAppconfig(AppConfig config);
        public  Task<Result> SetProperty(ConfigProperty property);
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

         if (json.IsFailure) return json;

         return await json.Map(json => json[nameof(property)] = property.ToString())
            .Bind((key) => _fs.WriteTextFileAsync(Path, json.ToString()));
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

        //public async Task<Result<T>> GetValue<T>(T property) where T : ConfigProperty
        //{
        //    //return await _fs.ReadTextFileAsync(_path)
        //    //      .Map(content => JObject.Parse(content))
        //    //      .Map(json => json[nameof(property)] = property.ToString())
        //    //      .Bind(json => _fs.WriteTextFileAsync(Path, json.ToString()));
        //}
    }

