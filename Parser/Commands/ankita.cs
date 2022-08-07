using BH.ErrorHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Parser.Commands
{
    internal class Ankita
    {
        public static void Decompose()
        {
            if (Parse.Ankita_isWaitingDEBUG)
            {
                if (Parse.wordLower == "debug")
                {
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = true;
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 8,
                        DevCode = 0,
                        ErrorMessage = "Ankita keyword is not found, this might be what you're looking for: 'Debug'.",
                        FilePath = Parse.masterPagePath,
                        line = Parse.line,
                        lineC = Parse.LineC,
                        lenC = Parse.TotalIndexOfLineWords
                    };
                    ErrorHandle.ErrorStack.PrintStack(new Error[] { err });
                    
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.Ankita_isWaitingBoolean)
            {
                if (Parse.wordLower == "on")
                {
                    Parse.Ankita_DEBUGBoolean = true;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = true;
                }
                else if (Parse.wordLower == "on;")
                {
                    Parse.Ankita_DEBUGBoolean = true;
                    Parse.Ankita_isWaitingBoolean = false;

                    LogSystem.DEBUG = Parse.Ankita_DEBUGBoolean;
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    LogSystem.log("debug mode enabled, Now you can see logs.", ConsoleColor.Green);
                }
                else if (Parse.wordLower == "off")
                {
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = true;
                }
                else if (Parse.wordLower == "off;")
                {
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingBoolean = false;

                    LogSystem.DEBUG = Parse.Ankita_DEBUGBoolean;
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    LogSystem.log("debug mode disabled, Now you can't see logs.", ConsoleColor.Green);
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 9,
                        DevCode = 0,
                        ErrorMessage = "Ankita value is not found, this might be what you're looking for: 'On' or 'Off'.",
                        FilePath = Parse.masterPagePath,
                        line = Parse.line,
                        lineC = Parse.LineC,
                        lenC = Parse.TotalIndexOfLineWords
                    };
                    ErrorHandle.ErrorStack.PrintStack(new Error[] { err });
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.Ankita_isWaitingEndKey)
            {
                if (Parse.word == ";")
                {
                    LogSystem.DEBUG = Parse.Ankita_DEBUGBoolean;

                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                    if (LogSystem.DEBUG) LogSystem.log("debug mode enabled, Now you can see logs.", ConsoleColor.Green);
                    else LogSystem.log("debug mode disabled, Now you can't see logs.", ConsoleColor.Green);
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 6,
                        DevCode = 0,
                        ErrorMessage = "End key not found. You should leave a space after ';' because of the word by word parse.",
                        FilePath = Parse.masterPagePath,
                        line = Parse.line,
                        lineC = Parse.LineC,
                        lenC = Parse.TotalIndexOfLineWords
                    };
                    ErrorHandle.ErrorStack.PrintStack(new Error[] { err });
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                }
            }
        }
    }
}
