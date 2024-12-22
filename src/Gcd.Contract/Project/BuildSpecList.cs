using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gcd.Contract.Project
{
    public class BuildSpecList
    {
        public const string COMMAND = "list";
        public const string COMMAND_DESCRIPTION = "List projects build spec";

        public const string PROJECT_PATH_OPTION = "--project-path";
        public const string PROJECT_PATH_DESCRIPTION = "Absolute path to a project";

        public const string SUCESS_MESSAGE = "Nipkg download succesfully pushed successully";
    }
}