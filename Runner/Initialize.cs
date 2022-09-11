using System.Text;
using ANSIConsole;
using BH.ErrorHandle;
using BH.Parser.Commands;

namespace BH.Runner
{
    public class Initialize
    {
        public static void Inıt_All()
        {
            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
            System.Console.OutputEncoding = Encoding.UTF8;
            Init_Cfg();
            Init_Logs();
            Init_Script();
            Init_ANSII();
            Init_Commands();
        }

        public static void Init_Commands()
        {
            printdbg.Init();
            app.Init();
            com.Init();
            compile.Init();
        }

        public static void Init_Cfg() => Parser.Config.Parser.Parse();
        
        public static void Init_Logs() => Logs.AllLogs = new StringBuilder();

        public static void Init_Script()
        {
            Script.Types.CF.Terminal.Input("@echo off");
            Script.Temp.LoadHashTemp();
        }

        public static void Init_ANSII() { if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false; }
        
        
    }
}