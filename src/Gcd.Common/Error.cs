using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Common
{
    public class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; }
        public int Code { get; set; }

        public override string ToString() => Message;
    }
}
