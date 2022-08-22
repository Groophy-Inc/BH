using BH.ErrorHandle;
using BH.ErrorHandle.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Structes.ErrorStack;

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
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);

                    Parse.EndProcess();
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

                    Logs.DEBUG = Parse.Ankita_DEBUGBoolean;
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Logs.Log("debug mode enabled, Now you can see logs.", ConsoleColor.Green);
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

                    Logs.DEBUG = Parse.Ankita_DEBUGBoolean;
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Logs.Log("debug mode disabled, Now you can't see logs.", ConsoleColor.Green);
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 9,
                        DevCode = 0,
                        ErrorMessage = "Ankita value is not found, this might be what you're looking for: 'On' or 'Off'.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Parse.EndProcess();
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.Ankita_isWaitingEndKey)
            {
                if (Parse.word == ";")
                {
                    Logs.DEBUG = Parse.Ankita_DEBUGBoolean;

                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Parse.EndProcess();
                    if (Logs.DEBUG) Logs.Log("debug mode enabled, Now you can see logs.", ConsoleColor.Green);
                    else Logs.Log("debug mode disabled, Now you can't see logs.", ConsoleColor.Green);
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 6,
                        DevCode = 0,
                        ErrorMessage = "End key not found. You should leave a space after ';' because of the word by word parse.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.Ankita_DEBUGBoolean = false;
                    Parse.Ankita_isWaitingDEBUG = false;
                    Parse.Ankita_isWaitingBoolean = false;
                    Parse.Ankita_isWaitingEndKey = false;
                    Parse.EndProcess();
                }
            }
        }
    }
}
