using CSharpFunctionalExtensions;
using Gcd.Nipkg.Instructions.Model;
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

    public Result<InstructionsFilePackage> AddCustomExecute(FilePackageCustomeExecute customExecute)
    {
        if(CustomExecutes == null)
        { 
            CustomExecutes = new CustomExecutes();
        }
        CustomExecutes.AddCustomExecute(customExecute);

        return Result.Success(this);

    }

}

public class CustomExecutes
{
    [XmlElement("customExecute")]
    public List<CustomExecute> CustomExecuteList { get; set; } = new List<CustomExecute>();
    public Result AddCustomExecute(FilePackageCustomeExecute customExecute)
    {
        if (CustomExecuteList == null)
        {
            CustomExecuteList = new List<CustomExecute>();
        }

        CustomExecuteList.Add(new CustomExecute{ 
            Arguments = customExecute.Arguments.Value,
            Root = customExecute.Root.Value,
            ExeName = customExecute.ExeName.Value,
            Step = customExecute.Step.Value,
            Schedule = customExecute.Schedule.Value
        });
        return Result.Success();
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

    [XmlAttribute("step")]
    public string Step { get; set; }

    [XmlAttribute("schedule")]
    public string Schedule { get; set; }
}

