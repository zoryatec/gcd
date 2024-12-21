using Gcd.Nipkg.Instructions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Gcd.Tests.UnitTest
{
    public class InstructionsSerializerTest
    {

        [Fact]
        public void SerializerCreatesDefaultContent_WhenNoParamtersSpecified()
        {
            var instr = new InstructionsFilePackage();

            var root = new CustomExecuteRoot("ddfdffdf");
            var args = new CustomExecuteArguments("dfdfff");
            var exeName = new CustomExecuteExeName("ddfdfssd");
            var step = new CustomExecuteStep("dfdfff");
            var schedule = new CustomExecuteSchedule("ddfdfssd");
            var custExe = new FilePackageCustomeExecute(root, exeName,args,step, schedule);
  

            instr.AddCustomExecute(custExe);

            var serailizer = new InstructionsSerializer();
            var serialized = serailizer.Serialize(instr);


            XDocument doc = XDocument.Parse(serialized.Value);

            // Query all customExecute elements
            var customExecutes = doc.Descendants("customExecute")
                                    .Select(x => new
                                    {
                                        Root = x.Attribute("root").Value,
                                        ExeName = x.Attribute("exeName").Value
                                    });

            var deserialized = serailizer.Deserialize(serialized.Value);

        }
    }
    
}
