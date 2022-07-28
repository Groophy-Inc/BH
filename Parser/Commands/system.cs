using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Parser.Commands
{
    public enum SystemCommands
    {
        NA,
        OutputEncoding
    }
    internal class system
    {
        public static void Decompose()
        {
            if (Parse.System_isPreloading)
            {
                switch (Parse.wordLower)
                {
                    case "outputencoding":
                        Parse.System_Command = SystemCommands.OutputEncoding;
                        Parse.System_isPreloading = false;
                        Parse.System_isWaitingValue = true;
                        break;
                }
            }
            else if (Parse.System_isWaitingValue)
            {
                if (Parse.word.EndsWith(';'))
                {
                    Parse.System_value.Add(Parse.word.Substring(0, Parse.word.Length-1));

                    //end
                    switch (Parse.System_Command)
                    {
                        case SystemCommands.OutputEncoding:
                            switch (Parse.System_value[0].ToLower())
                            {
                                case "uft-8":
                                case "uft8":
                                    Console.OutputEncoding = Encoding.UTF8;
                                    BH.ErrorHandle.LogSystem.log("New outputencoding: " + Parse.System_value[0].ToLower());
                                    break;
                                //Make more option here
                            }
                            break;
                    }

                    Parse.System_isPreloading = false;
                    Parse.System_isWaitingValue = false;
                    Parse.System_Command = SystemCommands.NA;
                    Parse.System_value = new List<string>();
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                }
                else
                {
                    Parse.System_value.Add(Parse.word);
                }
            }
        }
    }
}
