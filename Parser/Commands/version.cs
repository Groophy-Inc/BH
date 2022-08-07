using BH.ErrorHandle;
using System;

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
                    Console.WriteLine(Program.Ver);

                    Parse.Version_isWaitingEndKey = false;
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
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
                    ErrorHandle.ErrorStack.PrintStack(err);
                    Parse.Version_isWaitingEndKey = false;
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                }
            }
        }
    }
}
