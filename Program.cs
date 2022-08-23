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
using CSScriptLib;

namespace BH
{
    internal class Program
    {
        //0.2.9
        //Classification of voids
        //clearbhtemp argument
        //CSharp(cs) Executing Added to UMS, Output Type (Less test so can have some bugs)
        //Stdin, Stdout, Stderr, Stopwatch, isTimeout
        public static readonly string Ver = "0.2.9";
        static void Main(string[] args)
        {
            
            /*args = new string[]
            {
                //"--save",
                //"C:\\Users\\GROOPHY\\Desktop\\Logs.pdf",
                //"--debug",
                "--parse",
                @"C:\Users\GROOPHY\Desktop\umstest.txt",
                "--srcPath",
                @"C:\Users\GROOPHY\Desktop\",
                //"--checkhashforfastbuild",
            };*/
            
            
            APF.ArgumentParser.ParseArgs(args);
            if (APF.ArgumentParser.ParseFailed) return;
           
            Func<int> MainWorker = delegate()
            {
                Console.WriteLine(File.ReadAllText(Parse.masterPagePath)+"\r\n-------------------------");
                return 0;
            };
            
            Runner.Base.Run(MainWorker);
        }
    }
}
