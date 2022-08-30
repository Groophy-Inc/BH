using BH.ErrorHandle;
using System;
using ANSIConsole;
using BH.ErrorHandle.Error;
using BH.Structes.ErrorStack;

namespace BH.Parser.Commands
{
    internal class version
    {
        public static void Decompose()
        {
            if (Parse.Version_isWaitingEndKey)
            {
                if (Parse.word == ";")
                {
                    Console_.WriteLine(APF.Version.Read);

                    Parse.Version_isWaitingEndKey = false;
                    Logs.OnPropertyChanged("Version_isWaitingEndKey", false);
                    Parse.EndProcess();
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
                    ErrorStack.PrintStack(err,new System.Diagnostics.StackFrame(0, true));
                    Parse.Version_isWaitingEndKey = false;
                    Logs.OnPropertyChanged("Version_isWaitingEndKey", false);
                    Parse.EndProcess();
                }
            }
        }
    }
}
