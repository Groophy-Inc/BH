using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Parser.Commands
{
   

    public enum IScriptTypes
    {
        CF, //Terminal input     a.k.a CmdFunc
        BF, //Batch Script       a.k.a BatchFile
        SP, //Custom lang for BH a.k.a Script-Plus
        CS, //C# runtime parse   a.k.a C-Sharp[#]
    }

    public class IScript
    {
        public IScriptTypes Type { get; set; }

        /// <summary>
        /// is save output to a varriable 
        /// 
        /// Work for 
        ///   - CF
        ///   - BF
        ///   - SP
        /// </summary>
        public bool isCooking { get; set; } 
        public string Cook { get; set; }
        public string Code { get; set; }
    }

    internal class ums
    {

    }
}
