using BH.ErrorHandle;
using System.Text;
using ANSIConsole;

namespace BH
{
    public static class Console_
    {
        public static System.Text.StringBuilder ConsoleLogs = new StringBuilder();

        public static bool CanWriteToConsole = true;

        public static void Write(object text)
        {
            ConsoleLogs.Append(text.ToString().ClearANSII());
            
            if (!CanWriteToConsole) return;
            
            System.Console.Write(text);
        }

        public static void WriteLine(object text)
        {
            Write(text + "\r\n");
        }
    }
}
