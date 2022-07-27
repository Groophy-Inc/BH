using ANSIConsole;
namespace BH.ErrorHandle
{
    internal class DebugLogSystem
    {
        static System.Text.StringBuilder LogBuilder = new System.Text.StringBuilder();

        public static void Log(string text)
        {
            LogBuilder.Append(
                ClearANSII(
                    new ANSIConsole.ANSIString(text + "\r\n")
                    .Clear()
                    .ToString()
                    )
                );
        }

        public static void Clear()
        {
            LogBuilder.Clear();
        }

        public static void WriteAllText(string path)
        {
            System.IO.File.WriteAllText(path, LogBuilder.ToString());
        }

        public static string ClearANSII(string text)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            bool isANSII = false;
            for (int i = 0;i < text.Length;i++)
            {
                if (text[i] == '')
                {
                    isANSII = true;
                    continue;
                }
                else if (text[i] == 'm')
                {
                    if (isANSII) isANSII = false;
                    else sb.Append(text[i]);
                }
                else
                {
                    if (!isANSII) sb.Append(text[i]);
                }
            }
            return sb.ToString();
        }
    }
}
