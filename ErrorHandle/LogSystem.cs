using System;
using System.Text;
using System.Collections.Generic;
using ANSIConsole;
using BH.Parser.Utils;

namespace BH.ErrorHandle
{
    internal class LogSystem
    {
        static public bool DEBUG = false;
        static public bool NoSpace = false;

        static public void log(string text, ConsoleColor c = ConsoleColor.White)
        {
            text = text.Insert(0, "DBG - ").Replace("\n", "\nDBG - ");

            DebugLogSystem.Log(text);

            if (!DEBUG) return;
            if (NoSpace) text = text.Replace(' ', '_');

            var fc = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.WriteLine(text);
            Console.ForegroundColor = fc;
        }

        public static string BuildByStack(ADVLog[] logs)
        {
            //Freaking not work
            StringBuilder DevCode = new StringBuilder();
            StringBuilder ListedChar = new StringBuilder();
            List<int> lineCounts = new List<int>();
            string ErrLine = "";
            int errLine = 0;
            int start = 0;
            int len = 0;

            for (int i = 0; i < logs.Length; i++)
            {
                ADVLog err = logs[i];

                ListedChar.Append(err.line.Substring(err.lenC, 1));

                DevCode.Append(err.DevCode + "|");

                bool isFound = false;
                foreach (int x in lineCounts)
                {
                    if (err.lineC == x)
                    {
                        isFound = true;
                    }
                }
                if (!isFound) lineCounts.Add(err.lineC);


                if (i == 0)
                {
                    start = err.lenC;
                    len++;
                    ErrLine = err.line;
                    errLine = err.lineC;
                }
                else if (i + 1 == logs.Length)
                {
                    len++;
                    ErrLine = ("Ln" + err.lineC.ToString() + ": ").Color(ConsoleColor.Blue) + Color.ColorByIndex(ErrLine, start, len, ConsoleColor.Blue, System.ConsoleColor.Red).ToString();
                }
                else
                {
                    if (err.lineC == errLine)
                    {
                        if (err.lenC == start + len)
                        {
                            len++;
                        }
                        else
                        {
                            ErrLine = ("Ln" + err.lineC.ToString() + ": ").Color(ConsoleColor.Blue) + Color.ColorByIndex(ErrLine, start, len, ConsoleColor.Blue, System.ConsoleColor.Red).ToString();
                        }
                    }
                }
            }

            string lns = "";
            foreach (var x in lineCounts)
            {
                lns += x + ",";
            }
            lns = lns.Substring(0, lns.Length - 1);

            return $@"DEBUG LOG(Not Error)  - DevCode -> {DevCode.ToString().Substring(0, DevCode.Length - 1)} | Path '{logs[0].FilePath}'
{Color.ColorByIndex(logs[0].LogMessage, 0, System.ConsoleColor.Yellow)}
Ln: '{lns}' | ChLn: '{start}-{start + len}' | Ch: '{ListedChar.ToString().Color(ConsoleColor.Magenta)}'
{ErrLine}
";
        }
    }
}
