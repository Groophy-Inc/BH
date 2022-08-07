using System;
using ANSIConsole;
using System.Threading;
using BH.Parser;
using BH.ErrorHandle;
using System.Text;

namespace BH
{
    internal class Program
    {
        public static readonly string Ver = "0.1.5";

        static void Main(string[] args)
        {
            if (args.Length <= 2)
            {
                Parse.srcPath = "srcpath";
                Parse.masterPagePath = @"C:\Users\GROOPHY\Desktop\scripttest.txt";
                Parse.Options = new string[] { };
            }
            else
            {
                if (args[0] != "FABLWONT")
                {
                    Console_.WriteLine("You must " + "call BH.bat".Color(ConsoleColor.Yellow) + ", NOT BH.exe");
                    return;
                }
                Parse.srcPath = args[1];
                Parse.masterPagePath = args[2];
                if (args.Length > 3) Parse.Options = args[3..];
                else Parse.Options = new string[] { };
            }


            //LogSystem.DEBUG = true;




            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
            Console.Title = "BH - Varriable System";

            //Console_.WriteLine(System.IO.File.ReadAllText(Parse.masterPagePath) + "\r\n\r\n | \r\n\\_/ \r\n");

            //BH.APF.ConsoleHelper.SetCurrentFont("Consolas", 24);

            Parse.ParseLines(new string[]
            {
                "var     $hi       ->        \"Hello\"    ;      ",
                "msg       $hi      "
            });
            //Parser.Parse.ParseMasterPage();

           // Console.WriteLine("--\r\n" + Console_.ConsoleLogs.ToString());

            ErrorHandle.DebugLogSystem.WriteAllText(@"C:\Users\GROOPHY\Desktop\logs.txt");

        }
    }
}
