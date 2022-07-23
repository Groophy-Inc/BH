using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Parser
{
    public class Parse
    {
        public static string srcPath { get; set; }
        public static string masterPagePath { get; set; }
        public static string[] Options { get; set; }
        public static void ParseMasterPage(string _srcPath, string _masterPagePath, string[] _Options)
        {
            srcPath = _srcPath;
            masterPagePath = _masterPagePath;
            Options = _Options;
        }
    }
}
