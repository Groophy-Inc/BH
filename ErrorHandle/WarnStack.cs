using ANSIConsole;

namespace BH.ErrorHandle
{
    internal class WarnStack
    {
        public static void PrintStack(ADVLog[] warns)
        {
            LogSystem.BuildByStack(warns).Print();
        }
    }
    
    public class ADVLog
    {
        public ErrorPathCodes ErrorPathCode { get; set; }
        public int WarnID { get; set; }
        public int DevCode { get; set; }
        public string LogMessage { get; set; }
        public string FilePath { get; set; }
        public int lineC { get; set; } //Ln
        public int lenC { get; set; } //ChLn
        public string line { get; set; }
    }
}
