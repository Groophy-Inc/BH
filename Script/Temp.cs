using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Script
{
    internal class Temp
    {
        public static void RunAppByDotnet(bool printMS = false)
        {
            if (printMS)
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                var ot = CmdFunc.OneTimeInput("dotnet build", CF_Structes.ShellType.ChairmanandManagingDirector_CMD, APF.Helper.AssemblyDirectory + "\\Temp\\");
                RunApp();
                sw.Stop();
                Console.WriteLine("-" + sw.Elapsed.TotalMilliseconds + "ms-");
                ErrorHandle.Logs.Log("Dotnet build Stdout - \r\n" + ot.Std_Out.ToString());
            }
            else
            {
                CmdFunc.OneTimeInput("dotnet build", CF_Structes.ShellType.ChairmanandManagingDirector_CMD, APF.Helper.AssemblyDirectory + "\\Temp\\");
                RunApp();
            }
        }

        public static void RunApp()
        {
            System.Diagnostics.Process.Start(APF.Helper.AssemblyDirectory + $"\\Temp\\bin\\Debug\\net5.0-windows\\{Parser.Parse.ProjectName}.exe");
        }
    }
}
