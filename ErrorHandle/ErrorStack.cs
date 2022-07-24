using System;
using ANSIConsole;
namespace BH.ErrorHandle
{
    internal class ErrorStack
    {
        public static void PrintStack(Error[] errors)
        {
            //Console.WriteLine("new errlist count: " + errors.Length);
            //foreach (var x in errors)
            //{
            //    ErrMessageBuilder.Build(x).Print();
            //}
            ErrMessageBuilder.BuildByStack(errors).Print();
            
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
