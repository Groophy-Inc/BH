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
        public static string BuildByStack(Error[] errors)
        {
            StringBuilder DevCode = new StringBuilder();
            StringBuilder ListedChar = new StringBuilder();
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

//BH#1#0 - DevCode -> 0 | Path 'C:\Users\GROOPHY\Desktop\test.txt'
//Spilled out obscure object
//Ln: '1' | ChLn: '0-1' | Ch: '"'
//"title":a "world", //hello
            
        }
    }
}
