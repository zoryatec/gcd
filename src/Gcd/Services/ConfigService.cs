using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services
{
    public class ConfigService(IFileSystem _fs) : IConfigService
    {
        public Task<Result<AppConfig>> GetAppConfigAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result> SetAppconfig(AppConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
