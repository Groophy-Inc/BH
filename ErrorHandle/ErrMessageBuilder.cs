using BH.Parser.Utils;
using System.Text;
using System.Collections.Generic;
using System;
using ANSIConsole;
namespace BH.ErrorHandle
{
    public enum ErrorPathCodes
    {
        Program = 0,
        Parser = 1
    }
    internal class ErrMessageBuilder
    {
        public static string Build(ErrorPathCodes ErrorPathCode,
                                   int ErrorID,
                                   int DevCode,
                                   string ErrorMessage,
                                   string FilePath,
                                   int lineC, //Ln
                                   int lenC, //ChLn
                                   string line)
        {
            return $"BH#{(int)ErrorPathCode}#{ErrorID} - DevCode -> {DevCode} | Path '{FilePath}'\r\n{Color.ColorByIndex(ErrorMessage, 0, System.ConsoleColor.Yellow)}\r\nLn: '{lineC}' | ChLn: '{lenC}' | Ch: '{line.Substring(lenC, 1)}'\r\n{Color.ColorByIndex(line, lenC, 1, System.ConsoleColor.Red)}\r\n";
        }

        public static string Build(Error error)
        {
            return $"{$"BH#{(int)error.ErrorPathCode}#{error.ErrorID}".Color(ConsoleColor.Red)} - DevCode -> {error.DevCode} | Path '{error.FilePath}'\r\n{Color.ColorByIndex(error.ErrorMessage, 0, System.ConsoleColor.Yellow)}\r\nLn: '{error.lineC}' | ChLn: '{error.lenC}' | Ch: '{error.line.Substring(error.lenC, 1)}'\r\n{Color.ColorByIndex(error.line, error.lenC, 1, System.ConsoleColor.Red)}\r\n";
        }

        public static string BuildByStack(Error[] errors)
        {
            StringBuilder DevCode = new StringBuilder();
            StringBuilder ListedChar = new StringBuilder();
            //int currentLine = 0;
            List<int> lineCounts = new List<int>();
            string ErrLine = "";
            int errLine = 0;
            int start = 0;
            int len = 0;

            for (int i = 0; i < errors.Length; i++)
            {
                Error err = errors[i];

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
                else if (i + 1 == errors.Length)
                {

                    //last
                    len++;
                    ErrLine = ("Ln"+err.lineC.ToString() + ": ").Color(ConsoleColor.Blue) + Color.ColorByIndex(ErrLine, start, len, ConsoleColor.Blue, System.ConsoleColor.Red).ToString();
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

            return $@"{$"BH#{(int)errors[0].ErrorPathCode}#{errors[0].ErrorID}".Color(ConsoleColor.Red)} - DevCode -> {DevCode.ToString().Substring(0, DevCode.Length - 1)} | Path '{errors[0].FilePath}'
{Color.ColorByIndex(errors[0].ErrorMessage, 0, System.ConsoleColor.Yellow)}
Ln: '{lns}' | ChLn: '{start}-{start + len}' | Ch: '{ListedChar.ToString().Color(ConsoleColor.Magenta)}'
{ErrLine}
";
        }
    }
}
