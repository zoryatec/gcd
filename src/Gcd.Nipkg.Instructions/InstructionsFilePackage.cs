using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gcd.Tests.UnitTest;


[XmlRoot("instructions")]
public class InstructionsFilePackage
{
    [XmlElement("customExecutes")]
    public CustomExecutes CustomExecutes { get; set; }

    public void AddCustomExecute(CustomExecute customExecute)
    {
        if(CustomExecutes == null)
        { 
            CustomExecutes = new CustomExecutes();
        }
        CustomExecutes.AddCustomExecute(customExecute);
    }

}

public class CustomExecutes
{
    [XmlElement("customExecute")]
    public List<CustomExecute> CustomExecuteList { get; set; } = new List<CustomExecute>();
    public void AddCustomExecute(CustomExecute customExecute)
    {
        if (CustomExecuteList == null)
        {
            CustomExecuteList = new List<CustomExecute>();
        }
        CustomExecuteList.Add(customExecute);
    }


}

public class CustomExecute
{
    [XmlAttribute("root")]
    public string Root { get; set; }


    [XmlAttribute("arguments")]
    public string Arguments { get; set; } = default;

    [XmlAttribute("exeName")]
    public string ExeName { get; set; }
}

