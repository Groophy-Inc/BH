using BH.ErrorHandle;
using System.Text;
using ANSIConsole;

namespace BH
{
    public static class Console_
    {
        public static System.Text.StringBuilder ConsoleLogs = new StringBuilder();

        public static void Write(object text)
        {
            System.Console.Write(text);

            ConsoleLogs.Append(text.ToString().ClearANSII());
        }

        public static void WriteLine(object text)
        {
            System.Console.WriteLine(text);

            ConsoleLogs.Append(text.ToString().ClearANSII() + "\r\n");
        }
    }
}
