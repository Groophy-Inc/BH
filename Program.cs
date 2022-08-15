using System;
using ANSIConsole;
using System.Threading;
using BH.Parser;
using BH.ErrorHandle;
using System.Text;
using System.IO;
using System.Linq;
using System.Drawing;

namespace BH
{
    internal class Program
    {
        public static readonly string Ver = "0.1.6";

        static void Main(string[] args)
        {
            args = new string[]
            {
                "--save",
                "C:\\Users\\GROOPHY\\Desktop\\save.pdf",
                "--debug",
                "--parse",
                @"C:\Users\GROOPHY\Desktop\bigparsetest.txt",
                "--srcPath",
                @"C:\Users\GROOPHY\Desktop\",
                "--fablwont"
            };
            var ParsedArgs = APF.ArgumentParser.Parse(args);
            if (APF.ArgumentParser.ParseFailed) return;
            bool isFABLWONT = false;

            //C:\\Users\\GROOPHY\\Desktop\\save.pdf
            foreach (var arg in ParsedArgs)
            {
                var narg = new System.Collections.Generic.KeyValuePair<string, string>(arg.Key.Replace('I','i'), arg.Value);
                if (narg.Key == "s" || narg.Key == "save")
                {
                    SavePath = narg.Value;
                }
                else if (narg.Key == "debug")
                {
                    Logs.DEBUG = true;
                }
                else if (narg.Key == "fablwont")
                {
                    isFABLWONT |= true;
                }
                else if (narg.Key == "parse" || narg.Key == "p")
                {
                    Parse.masterPagePath = narg.Value;
                }
                else if (narg.Key == "srcpath" || narg.Key == "src")
                {
                    Parse.srcPath = narg.Value;
                }
            }

            if (!isFABLWONT)
            {
                Console_.WriteLine("You must " + "call BH.bat".Color(ConsoleColor.Yellow) + ", NOT BH.exe");
                return;
            }

            if (args.Length == 0)
            {
                ANSIIConsole.Gecho.Print(@"<#2f2f8a>BH <w>[-s|--save <#af916d>\<SAVE PATH\><w>] <#18cff2>[--debug] <w>[-p|--parse <#af916d>\<MASTER PAGE PATH\><w>] <w>[-src|--srcpath <#af916d>\<SRCPATH WHERE HAVE YOUR TOOLS\><w>] <#18cff2>[--fablwont]");
            }
            Logs.AllLogs = new StringBuilder();


            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
            Console.Title = "BH - Varriable System";

            Builder.HightLightPack[] hl = new Builder.HightLightPack[]
            {
                new Builder.HightLightPack()
                {
                    KeyWords = new string[]{"class", "string", "namespace", "using"},
                    HexColor = "84dcfa"
                },
                new Builder.HightLightPack()
                {
                    KeyWords = new string[]{"public" },
                    HexColor = "1f3065"
                }
            };
            var ns = new Structes.BodyClasses._Namespace()
            {
                Name = "Test",
                Using = new System.Collections.Generic.List<string>(new string[] { "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks", "System.Windows", "System.Windows.Controls", "System.Windows.Data",
                "System.Windows.Documents","System.Windows.Input","System.Windows.Media","System.Windows.Media.Imaging","System.Windows.Navigation","System.Windows.Shapes"}), 
                Classes = new System.Collections.Generic.List<Structes.BodyClasses._Class>()
                {
                    new Structes.BodyClasses._Class()
                    {
                        Name = "ExampleClass",
                        Voides = new System.Collections.Generic.List<Structes.BodyClasses._Void>()
                        {
                            new Structes.BodyClasses._Void()
                            {
                                isField = true,
                                Access = "public",
                                Name = "Name",
                                ReturnType = "string",
                                FieldDefualt = "{get;set;}"
                            },
                            new Structes.BodyClasses._Void()
                            {
                                Access = "public",
                                Args = new System.Collections.Generic.List<string>(),
                                Code = @"
Console.WriteLine(" + "Hello World" + @");
",
                                Name = "Say"
                            }
                        }
                    }
                }
            };

            //Console.WriteLine(Builder.Build.Init(ns) + "\r\n-----------------------");

            //Console.WriteLine(Builder.Build.HighLightBracket(Builder.Build.Init(ns), hl));

            APF.ParalelPrint.Print(Builder.Build.Init(ns), Builder.Build.HighLightBracket(Builder.Build.Init(ns), hl));

            //File.WriteAllText("save.txt", Builder.Build.HighLightBracket(Builder.Build.Init(ns), hl));
            //Parser.Parse.ParseMasterPage();

            //Save();
        }

        public static string SavePath = "";
        public static void Save()
        {
            if (string.IsNullOrEmpty(SavePath)) return;
            Console.WriteLine("Logs saving...");

            File.WriteAllText("Input.txt", Logs.AllLogs.ToString());

            ClearCurrentConsoleLine(2); Console.WriteLine("Logs printing to pdf.");

            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "text2pdf/text2pdf.exe";
            p.StartInfo.Arguments = $"\"{APF.Helper.AssemblyDirectory}\\Input.txt\" \"{SavePath}\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            ClearCurrentConsoleLine(2); Console.WriteLine("Logs Saved.");

        }
        public static void ClearCurrentConsoleLine(int DelLineCount = 0)
        {
            for (int i = 1; i < DelLineCount; i++)
            {
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop - i);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }
            Console.SetCursorPosition(0, Console.CursorTop - DelLineCount + 1);
        }
    }
}
