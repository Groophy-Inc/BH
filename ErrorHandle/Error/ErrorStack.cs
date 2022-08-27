using System;
using System.Data;
using System.Net;
using ANSIConsole;
using BH.ErrorHandle.Error;
using BH.Parser;
using BH.Structes.ErrorStack;

namespace BH.ErrorHandle.Error
{
    internal class ErrorStack
    {
        public static void PrintStack(Structes.ErrorStack.Error error, string where)
        {
            try
            {
                DetailedError errdet = new DetailedError()
                {
                    ErrorID = error.ErrorID,
                    DevCode = error.DevCode,
                    ErrorMessage = error.ErrorMessage,
                    ErrorPathCode = error.ErrorPathCode,
                    ErrPath = Parse.ErrPath,
                    HighLightLen = error.HighLightLen,
                    line = error.line,
                    LineC = Parse.LineC,
                    TotalIndexOfLineWords = Parse.TotalIndexOfLineWords,
                    Date = DateTime.Now.ToString("HH:mm:ss")
                };
                string err = ErrMessageBuilder.BuildByStack(errdet, where);
                Parse.logErrMsg += err.ClearANSII();
                Logs.Log("\r\n" + err + "\r\n");
                err.Print();
            }
            catch (Exception e)
            {
                Console_.WriteLine("This error was written manually because an error occurred in the error printing.".Color(ConsoleColor.Magenta));
                Console_.WriteLine($@"BH#2#0 - DevCode -> 0 | Path 'Unknown'
Stderr: {e.Message}"); 
                Environment.Exit(2);
            }
        }
    }
}
