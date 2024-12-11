using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model
{
    public record InstructionFileContent
    {
        public override string ToString() =>
 @"<instructions>
	<targetAttributes readOnly=""allWritable""/>
    <customExecutes>
        <customExecute root=""BootVolume"" step=""install"" schedule=""post"" exeName=""Program Files\gcd\gcd.exe"" arguments=""system add-to-user-path --path C:\PROGRA~1\gcd"" />
    </customExecutes>
</instructions>
";
    }
}
