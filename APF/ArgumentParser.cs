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
        public static bool isCheckHashForFastBuild = false;

        public static bool isParsedMasterPage = false;
        public static bool isParsedSrcPath = false;

        public static bool Silence = false;
        public static bool dotout = false;

        public static void ParseArgs(bool debug)
        {
            string[] args = new String[] { };
            if (debug)
                args = new string[]
                {
                    "--save",
                    "C:\\Users\\GROOPHY\\Desktop\\Logs.pdf",
                    "--Silence",
                    //"--debug",
                    "--parse",
                    @"C:\Users\GROOPHY\Desktop\desktop\Code\Batch\BH_2\BH\Test.BH",
                    "--srcPath",
                    @"C:\Users\GROOPHY\Desktop\desktop\Code\Batch\BH_2\Src\",
                    //"--checkhashforfastbuild",
                    "--dotout"
                };
            else
            {
                args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            }

            var ParsedArgs = Parse(args);
            foreach (var arg in ParsedArgs)
            {
                var narg = new System.Collections.Generic.KeyValuePair<string, string>(
                    arg.Key.Replace('I', 'i').ToLower(), arg.Value);

                if (narg.Key == "s" || narg.Key == "save")
                {
                    APF.SaveSystem.SavePath = narg.Value;
                }
                else if (narg.Key == "debug")
                {
                    Logs.DEBUG = true;
                }
                else if (narg.Key == "parse" || narg.Key == "p")
                {
                    Parser.Parse.masterPagePath = narg.Value;
                    isParsedMasterPage = true;
                }
                else if (narg.Key == "srcpath" || narg.Key == "src")
                {
                    string src = arg.Value;
                    src = fixone(src);
                    Parser.Parse.srcPath = src;
                    Varriables.AddorUpdate("srcpath", src);
                    isParsedSrcPath = true;
                }
                else if (narg.Key == "checkhashforfastbuild" || narg.Key == "chffb")
                {
                    isCheckHashForFastBuild = true;
                }
                else if (narg.Key == "clearbhtemp" || narg.Key == "cbt")
                {
                    Script.Temp.ClearTemp();
                }
                else if (narg.Key == "silence")
                {
                    Silence = true;
                }
                else if (narg.Key == "dotout")
                {
                    dotout = true;
                }
            }

            if (!APF.ArgumentParser.isParsedMasterPage && !APF.ArgumentParser.isParsedSrcPath)
            {
                ParseFailed = true;
                ANSIIConsole.Gecho.Print(
                    @"<#2f2f8a>BH <w>[-s|--save <#af916d>\<SAVE PATH\><w>] <#18cff2>[--debug] <r>!<w>[-p|--parse <#af916d>\<MASTER PAGE PATH\><w>] <r>!<w>[-src|--srcpath <#af916d>\<SRCPATH WHERE HAVE YOUR TOOLS\><w>] <#18cff2>[--checkhashforfastbuild|-chffb] <#18cff2>[--ClearBHTemp|-cbt] <#18cff2>[--dotout]");
            }

            if (ParseFailed) Environment.Exit(-1);
        }

        public static string fixone(string text)
        {
            return text.Replace(@"\", @"\\").Replace(@"\\\\", @"\\");
        }

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