using System;

namespace BH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ErrorHandle.LogSystem.DEBUG = true;

            if (args.Length == 0) args = new string[] { @"C:\Users\GROOPHY\Desktop\desktop\Code\Batch\BH_2\src", @"C:\Users\GROOPHY\Desktop\desktop\Code\Batch\BH_2\src\Master.gui", "/t" };

            string srcPath = args[0];
            string masterPagePath = args[1];
            string[] Options = args[2..];

            Parser.Parse.ParseMasterPage(srcPath, masterPagePath, Options);
            
            
        }
    }
}
