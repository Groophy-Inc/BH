using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANSIConsole;

namespace BH.ErrorHandle
{
    public class Logs
    {
        public static bool isTab = false;

        public static StringBuilder AllLogs;

        static public bool DEBUG = false;
        static public bool NoSpace = false;

        public static void Log(object text, ConsoleColor c=ConsoleColor.White)
        {
            if (isTab) AllLogs.Append(text.ToString().ClearANSII().Insert(0, "      ").Replace("\n", "\n      ") + "\r\n");
            else AllLogs.Append(text.ToString().ClearANSII()+"\r\n");

            text = text.ToString().Insert(0, "DBG - ").Replace("\n", "\nDBG - ");
            
            if (!DEBUG) return;
            if (NoSpace) text = text.ToString().Replace(' ', '_');

            var fc = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.WriteLine(text);
            Console.ForegroundColor = fc;
        }

        public static void LogW(object text, ConsoleColor c = ConsoleColor.White)
        {
            if (isTab) AllLogs.Append(text.ToString().ClearANSII());
            else AllLogs.Append(text.ToString().ClearANSII());

            text = text.ToString().Insert(0, "DBG - ").Replace("\n", "\nDBG - ");

            if (!DEBUG) return;
            if (NoSpace) text = text.ToString().Replace(' ', '_');

            var fc = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.WriteLine(text);
            Console.ForegroundColor = fc;
        }

        public static void LogWW(object text, ConsoleColor c = ConsoleColor.White)
        {
            if (isTab) AllLogs.Append(text.ToString().ClearANSII().Insert(0, "      ").Replace("\n", "\n      "));
            else AllLogs.Append(text.ToString().ClearANSII());

            text = text.ToString().Insert(0, "DBG - ").Replace("\n", "\nDBG - ");

            if (!DEBUG) return;
            if (NoSpace) text = text.ToString().Replace(' ', '_');

            var fc = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.Write(text);
            Console.ForegroundColor = fc;
        }

        public static void OnPropertyChanged(string propertyName, object newvalue)
        {
            Log(propertyName + " = " + newvalue + ";");
        }
    }
}
