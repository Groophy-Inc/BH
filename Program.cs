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


            Parser.Parse.ParseMasterPage("", @"C:\Users\GROOPHY\Desktop\BH.txt", new string[] { });
            Console.ReadKey();
            /*
            if (args.Length == 0) args = new string[] { @"C:\Users\GROOPHY\Desktop\desktop\Code\Batch\BH_2\src", @"C:\Users\GROOPHY\Desktop\desktop\Code\Batch\BH_2\src\Master.gui", "/t" };

            string srcPath = args[0];
            string masterPagePath = args[1];
            string[] Options = args[2..];

            Parser.Parse.ParseMasterPage(srcPath, masterPagePath, Options);
            
            */
        }
    }
}
