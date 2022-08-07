using BH.ErrorHandle;
using System.Text;

namespace BH
{
    public static class Console_
    {
        public static System.Text.StringBuilder ConsoleLogs = new StringBuilder();

        public static void Write(object text)
        {
            System.Console.Write(text);

            ConsoleLogs.Append(DebugLogSystem.ClearANSII(text.ToString()));
        }

        public static void WriteLine(object text)
        {
            System.Console.WriteLine(text);

            ConsoleLogs.Append(DebugLogSystem.ClearANSII(text.ToString()) + "\r\n");
        }
    }
}
