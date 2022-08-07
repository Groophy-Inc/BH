using System;
using ANSIConsole;
namespace BH.ErrorHandle
{
    internal class ErrorStack
    {
        public static void PrintStack(Error error)
        {
            DetailedError errdet = new DetailedError()
            {
                ErrorID = error.ErrorID,
                DevCode = error.DevCode,
                ErrorMessage = error.ErrorMessage,
                ErrorPathCode = error.ErrorPathCode,
                ErrPath = Parser.Parse.ErrPath,
                HighLightLen = error.HighLightLen,
                line = error.line,
                LineC = Parser.Parse.LineC,
                TotalIndexOfLineWords = Parser.Parse.TotalIndexOfLineWords
            };
            string err = ErrMessageBuilder.BuildByStack(errdet);
            Parser.Parse.logErrMsg += err.ClearANSII();
            err.Print();
        }
    }

    public class Error
    {
        public ErrorPathCodes ErrorPathCode { get; set; }
        public int ErrorID { get; set; }
        public int DevCode { get; set; }
        public string ErrorMessage { get; set; }
        public int HighLightLen { get; set; }
        public string line { get; set; }
    }

    public class DetailedError
    {
        public ErrorPathCodes ErrorPathCode { get; set; }
        public int ErrorID { get; set; }
        public int DevCode { get; set; }
        public string ErrorMessage { get; set; }
        public int HighLightLen { get; set; }
        public string line { get; set; }
        public string ErrPath { get; set; }
        public int TotalIndexOfLineWords { get; set; }
        public int LineC { get; set; }

    }
}
