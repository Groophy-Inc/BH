using ANSIConsole;
using BH.ErrorHandle;
using BH.Parser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.APF
{
    internal class ArgumentParser
    {
        public static bool ParseFailed = false;
        public static Dictionary<string, string> Parse(string[] args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string Name = "";
            string Value = "";
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                
                if (arg.StartsWith('-'))
                {
                    if (i != 0) result.Add(Name, Value);
                    Name = arg.TrimStart('-');
                    Value = "";
                }
                else
                {
                    if (string.IsNullOrEmpty(Value))
                    {
                        Value = arg;
                    }
                    else
                    {
                        $@"{$"BH#1#11".Color(ConsoleColor.Blue)} - DevCode -> 0 | Path 'Argument Parser'
{Color.ColorByIndex("UnAcceptable argument", 0, System.ConsoleColor.Yellow)}
Ln: '0' | ChLn: '-1 - -1' | Ch: '{arg.Color(ConsoleColor.Magenta)}' | Time: {DateTime.Now.ToString("HH:mm:ss")}  
{string.Join(' ', args)}".Print();
                        ParseFailed = true;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(Name)) result.Add(Name, Value);
            return result;
        }
    }
}
