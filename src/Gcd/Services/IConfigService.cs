using CSharpFunctionalExtensions;
using Gcd.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Services
{
    public interface IConfigService
    {
        public Task<Result<AppConfig>> GetAppConfigAsync();
        public Task<Result> SetAppconfig(AppConfig config);
    }
}
