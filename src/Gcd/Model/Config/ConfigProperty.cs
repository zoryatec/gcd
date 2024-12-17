using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Config
{
    public abstract record ConfigProperty
    {
        protected ConfigProperty(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
