using ANSIConsole;
using BH.ErrorHandle;
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
        OutputEncoding,
        Title
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
                    case "title":
                        Parse.System_Command = SystemCommands.Title;
                        Parse.System_isPreloading = false;
                        Parse.System_isWaitingValue = true;
                        break;
                }
            }
            else if (Parse.System_isWaitingValue)
            {
                if (Parse.word.EndsWith(';'))
                {
                    Parse.System_value.Add(Varriables.FixContent(Parse.word.Substring(0, Parse.word.Length - 1)));

                    //end
                    switch (Parse.System_Command)
                    {
                        case SystemCommands.OutputEncoding:
                            switch (Parse.System_value[0].Replace('I', 'i').ToLower())
                            {
                                case "uft-8":
                                case "uft8":
                                    Console.OutputEncoding = Encoding.UTF8;
                                    BH.ErrorHandle.LogSystem.log("New outputencoding: " + Parse.System_value[0].ToLower());
                                    break;
                                case "ascii":
                                    Console.OutputEncoding = Encoding.ASCII;
                                    BH.ErrorHandle.LogSystem.log("New outputencoding: " + Parse.System_value[0].ToLower());
                                    break;
                                case "unicode":
                                    Console.OutputEncoding = Encoding.Unicode;
                                    BH.ErrorHandle.LogSystem.log("New outputencoding: " + Parse.System_value[0].ToLower());
                                    break;
                                //Make more option here
                                default:

                                    //ERROR
                                    string getClosest = APF.Find_Probabilities.GetClosest(Parse.System_value[0], new string[] {"uft-8", "uft8", "ansii", "unicode"});
                                    Error err = new Error()
                                    {
                                        ErrorPathCode = ErrorPathCodes.Parser,
                                        ErrorID = 7,
                                        DevCode = 0,
                                        ErrorMessage = "Encoding is not found, this might be what you're looking for: '" + getClosest.Color(ConsoleColor.Green) + "'.",
                                        FilePath = Parse.masterPagePath,
                                        line = Parse.line,
                                        lineC = Parse.LineC,
                                        lenC = Parse.TotalIndexOfLineWords
                                    };
                                    ErrorHandle.ErrorStack.PrintStack(new Error[] { err });
                                    Parse.System_isPreloading = false;
                                    Parse.System_isWaitingValue = false;
                                    Parse.System_Command = SystemCommands.NA;
                                    Parse.System_value = new List<string>();
                                    Parse.isProgress = false;
                                    Parse.ProgressSyntax = "";
                                    Parse.isBackslashableContent = false;

                                    break;
                            }
                            break;
                        case SystemCommands.Title:
                            Console.Title = String.Join(' ', Parse.System_value.ToArray());
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
                    Parse.System_value.Add(Varriables.FixContent(Parse.word));
                }
            }
        }
    }
}
