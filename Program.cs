using System;
using ANSIConsole;
using BH.Parser;

namespace BH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ErrorHandle.LogSystem.DEBUG = true;
            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;

            //Console.WriteLine(System.IO.File.ReadAllText(@"C:\Users\GROOPHY\Desktop\msgtest.txt") + "\r\n | \r\n\\_/ \r\n");


            Parser.Parse.ParseMasterPage("", @"C:\Users\GROOPHY\Desktop\msgtest.txt", new string[] { });

            ErrorHandle.DebugLogSystem.WriteAllText(@"C:\Users\GROOPHY\Desktop\logs.txt");
            Console.ReadKey();
        }
    }
}
