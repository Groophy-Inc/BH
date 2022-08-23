using System.Text;
using ANSIConsole;
using BH.ErrorHandle;

namespace BH.Runner
{
    public class Initialize
    {
        public static void Inıt_All()
        {
            Init_Logs();
            Init_Script();
            Init_ANSII();
        }
        public static void Init_Logs() => Logs.AllLogs = new StringBuilder();

        public static void Init_Script()
        {
            Script.Types.CF.Terminal.Input("@echo off");
            Script.Temp.LoadHashTemp();
        }

        public static void Init_ANSII() { if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false; }
        
        
    }
}