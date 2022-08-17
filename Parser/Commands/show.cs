using BH.ErrorHandle.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Parser.Commands
{
    internal class Show
    {
        public static void Decompose()
        {
            if (Parse.Show_isWaitingVarriable)
            {
                if (Parse.wordLower.StartsWith('$'))
                {
                    var varName = Parse.word.Substring(1).TrimEnd(';');
                    Builder.Build.Demo(Varriables.TryGet(varName).Obj);
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 2,
                        DevCode = 0,
                        ErrorMessage = "Varriable not found, You may have forgotten to put '$'.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Parse.Show_isWaitingVarriable = false;
                }
            }
        }
    }
}

