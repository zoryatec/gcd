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
            var custExe = new CustomExecutes();

            instr.AddCustomExecute(new CustomExecute { Arguments = "dfdf", ExeName = "ddfdf", Root = "dfdsfd" });

            var serailizer = new InstructionsSerializer();
            var serialized = serailizer.Serialize(instr);


            XDocument doc = XDocument.Parse(serialized);

            // Query all customExecute elements
            var customExecutes = doc.Descendants("customExecute")
                                    .Select(x => new
                                    {
                                        Root = x.Attribute("root").Value,
                                        ExeName = x.Attribute("exeName").Value
                                    });

            var deserialized = serailizer.Deserialize(serialized);

        }
    }
    
}
