using System;
using ANSIConsole;
namespace BH.ErrorHandle
{
    internal class ErrorStack
    {
        public static void PrintStack(Error[] errors)
        {
            string err = ErrMessageBuilder.BuildByStack(errors);
            Parser.Parse.logErrMsg += err.ClearANSII();
            err.Print();
        }

        public static void PrintStackAsWord(Error error, string word)
        {

        }
    }

    public class Error
    {
        public ErrorPathCodes ErrorPathCode { get; set; }
        public int ErrorID { get; set; }
        public int DevCode { get; set; }
        public string ErrorMessage { get; set; }
        public string FilePath { get; set; }
        public int lineC { get; set; } //Ln
        public int lenC { get; set; } //ChLn
        public string line { get; set; }
    }
}
