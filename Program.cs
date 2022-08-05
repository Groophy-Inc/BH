using System;
using ANSIConsole;
using System.Threading;
using BH.Parser;
using BH.ErrorHandle;

namespace BH
{
    internal class Program
    {
        static  void Main(string[] args)
        {
            if (args.Length == 0) args = new string[3] { "FABLWONT", "srcpath", @"C:\Users\GROOPHY\Desktop\scripttest.txt"};
            if (args.Length > 2)
            {
                if (args[0] != "FABLWONT")
                {
                    Console.WriteLine("You must " + "call BH.bat".Color(ConsoleColor.Yellow)+", NOT BH.exe");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Invalid argumants");
                return;
            }

            LogSystem.DEBUG = true;
            Parse.srcPath = args[1];
            Parse.masterPagePath = args[2];
            if (args.Length > 3) Parse.Options = args[3..];
            else Parse.Options = new string[] { };


            /*System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            LogSystem.log("Checking winget...");
            LogSystem.log("isWingetFound: "+APF.Validate.isHaveWinget());

            LogSystem.log(" ");

            LogSystem.log("Checking dotnet...");
            LogSystem.log("isDotnetFound: " + APF.Validate.isHaveDotnet());

            LogSystem.log(" ");

            LogSystem.log("Checking NET5...");
            LogSystem.log("isNET5Found: " + APF.Validate.isHaveNET5());

            sw.Stop();

            Console.WriteLine("end - " + sw.Elapsed.TotalMilliseconds+"MS");
            Console.ReadKey();*/

            

            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
            Console.Title = "BH - Varriable System";

            Console.WriteLine(System.IO.File.ReadAllText(Parse.masterPagePath) + "\r\n\r\n | \r\n\\_/ \r\n");

            //BH.APF.ConsoleHelper.SetCurrentFont("Consolas", 24);

            Parser.Parse.ParseMasterPage();

            ErrorHandle.DebugLogSystem.WriteAllText(@"C:\Users\GROOPHY\Desktop\logs.txt");

            Console.ReadKey();
        }
    }
}
