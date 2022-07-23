using Newtonsoft.Json;
using System.IO;
using BH.Structes;

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

            GetElements.Get = JsonConvert.DeserializeObject<AllElements>(File.ReadAllText(masterPagePath)).ElementList;

                        
        }
    }
}
