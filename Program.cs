using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ANSIConsole;
using System.Threading;
using BH.Parser;
using BH.ErrorHandle;
using System.Text;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using BH.Structes;
using CSScriptLib;

namespace BH
{
    internal class Program
    {
        public static readonly string Ver = "0.3.6";
        
        static async Task Main(string[] args) 
        {
            
            args = new string[]
            {
                //"--save",
                //"C:\\Users\\GROOPHY\\Desktop\\Logs.pdf",
                //"--debug",
                "--parse",
                @"C:\Users\GROOPHY\Desktop\ums2.txt",
                "--srcPath",
                @"C:\Users\GROOPHY\Desktop\desktop\Code\Batch\BH_2\Src\",
                //"--checkhashforfastbuild",
            };
            
            
            APF.ArgumentParser.ParseArgs(args);
            if (APF.ArgumentParser.ParseFailed) return;


            Func<int> MainWorker = delegate()
            {
                Console.WriteLine(File.ReadAllText(Parse.masterPagePath)+"\r\n-------------------------");
                return 0;
            };
            
            var _Runner = await Runner.Base.Run(MainWorker);
            
        }
    }
}
