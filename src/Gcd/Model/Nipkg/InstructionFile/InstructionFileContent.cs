using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Model.Nipkg.InstructionFile
{
    public record InstructionFileContent
    {
        public static InstructionFileContent Default => new InstructionFileContent();
        public override string ToString() =>
 @"<instructions>
	<targetAttributes readOnly=""allWritable""/>
</instructions>
";

        // @"<instructions>
        //	<targetAttributes readOnly=""allWritable""/>
        //    <customExecutes>
        //        <customExecute root=""BootVolume"" step=""install"" schedule=""post"" exeName=""Program Files\gcd\gcd.exe"" arguments=""tools add-to-user-path C:\PROGRA~1\gcd"" />
        //    </customExecutes>
        //</instructions>
        //";
    }
}
