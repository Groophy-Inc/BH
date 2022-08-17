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
        public static readonly string Ver = "0.1.8";
        public static bool isCheckHashForFastBuild = false;
        public static string LastestHash = "NaN";

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
                "--checkhashforfastbuild",
            };
            var ParsedArgs = APF.ArgumentParser.Parse(args);
            if (APF.ArgumentParser.ParseFailed) return;

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
                else if (narg.Key == "parse" || narg.Key == "p")
                {
                    Parse.masterPagePath = narg.Value;
                }
                else if (narg.Key == "srcpath" || narg.Key == "src")
                {
                    Parse.srcPath = narg.Value;
                }
                else if (narg.Key == "checkhashforfastbuild" || narg.Key == "chffb")
                {
                    isCheckHashForFastBuild = true;
                }
            }


            if (args.Length == 0 || args.Length == 1)
            {
                ANSIIConsole.Gecho.Print(@"<#2f2f8a>BH <w>[-s|--save <#af916d>\<SAVE PATH\><w>] <#18cff2>[--debug] <w>[-p|--parse <#af916d>\<MASTER PAGE PATH\><w>] <w>[-src|--srcpath <#af916d>\<SRCPATH WHERE HAVE YOUR TOOLS\><w>] <#18cff2>[--fablwont]");
            }
            Logs.AllLogs = new StringBuilder();


            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
            if (File.Exists(APF.Helper.AssemblyDirectory + "\\LastestHash.hash")) LastestHash = File.ReadAllText(APF.Helper.AssemblyDirectory + "\\LastestHash.hash");
            if (File.Exists(APF.Helper.AssemblyDirectory + "\\LastestProjectName")) Parse.ProjectName = File.ReadAllText(APF.Helper.AssemblyDirectory + "\\LastestProjectName");
            Console.Title = "BH - ThinkNo";

            //File.WriteAllText("save.txt", Builder.Build.HighLightBracket(Builder.Build.Init(ns), Builder.Build.hl).ClearANSII());
            Console.WriteLine(File.ReadAllText(Parse.masterPagePath)+"\r\n-------------------------");

            if (isCheckHashForFastBuild)
            {
                if (LastestHash == HashString(File.ReadAllText(Parse.masterPagePath)))
                {
                    Script.Temp.RunApp();
                }
                else
                {
                    Parser.Parse.ParseMasterPage();
                }
            }
            else
            {
                Parser.Parse.ParseMasterPage();
            }

            File.WriteAllText(APF.Helper.AssemblyDirectory + "\\LastestHash.hash", HashString(File.ReadAllText(Parse.masterPagePath)));
            File.WriteAllText(APF.Helper.AssemblyDirectory + "\\LastestProjectName", Parse.ProjectName);

            Save();
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

        static string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
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
