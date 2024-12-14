using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model
{
    public interface IControlPropertyFactory
    {
        Result<ControlFileProperty> Create(string key, string value);
    }


    public class ControlPropertyFactory
    {
        Result<ControlFileProperty> Create(string key, string value)
        {
            return Result.Failure<ControlFileProperty>("not impl");
        }
    }
}
