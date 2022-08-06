using System;
using ANSIConsole;
using System.Threading;
using BH.Parser;
using BH.ErrorHandle;

namespace BH
{
    internal class Program
    {
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
                    Console.WriteLine("You must " + "call BH.bat".Color(ConsoleColor.Yellow) + ", NOT BH.exe");
                    return;
                }
                Parse.srcPath = args[1];
                Parse.masterPagePath = args[2];
                if (args.Length > 3) Parse.Options = args[3..];
                else Parse.Options = new string[] { };
            }


            LogSystem.DEBUG = true;




            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
            Console.Title = "BH - Varriable System";

            //Console.WriteLine(System.IO.File.ReadAllText(Parse.masterPagePath) + "\r\n\r\n | \r\n\\_/ \r\n");

            //BH.APF.ConsoleHelper.SetCurrentFont("Consolas", 24);


            //Parse.ParseLine("system outputencoding ufp-8;");
            Parser.Parse.ParseMasterPage();

            ErrorHandle.DebugLogSystem.WriteAllText(@"C:\Users\GROOPHY\Desktop\logs.txt");

            Console.ReadKey();
        }
    }
}
