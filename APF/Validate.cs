using System;
using System.IO;
using System.Net;
using ANSIConsole;

namespace BH.APF
{
    internal class Validate
    {
        private static CmdFunc tempcmd = new CmdFunc(Path.GetTempPath(), CF_Structes.ShellType.ChairmanandManagingDirector_CMD, false);

        public static bool isHaveWinget()
        {
            CmdFunc.Grp getVer = tempcmd.Input("winget -v");
            if (getVer.Stdout.ToString().Trim().StartsWith("v"))
            {
                return true;
            }
            return false;
        }

        public static void InstallWinget()
        {
            if (File.Exists(Path.GetTempPath()+"Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"))
            {
                File.Delete(Path.GetTempPath() + "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle");
            }
            using (var client = new WebClient())
            {
                client.DownloadFile("https://github.com/microsoft/winget-cli/releases/download/v1.3.2091/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle", Path.GetTempPath() + "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle");
            }
            tempcmd.Input("call \"Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle\"");
            Console_.WriteLine("Please type (C)ontinue when you install winget");
            ConsoleKey lt = ConsoleKey.Z;
            while (lt != ConsoleKey.C)
            {
                lt = Console.ReadKey().Key;
            }
            Console_.Write("\r\n");
        }

        public static bool isHaveDotnet()
        {
            CmdFunc.Grp getVer = tempcmd.Input("dotnet --version");
            if (char.IsDigit(getVer.Stdout.ToString().Trim(), 0))
            {
                return true;
            }
            return false;
        }

        public static void InstallDotnet()
        {
            if (File.Exists(Path.GetTempPath() + "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"))
            {
                File.Delete(Path.GetTempPath() + "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle");
            }
            File.WriteAllText(Path.GetTempPath()+"InstallDotnet.bat", "winget install -e --id Microsoft.dotnet");
            System.Diagnostics.Process.Start(Path.GetTempPath()+"InstallDotnet.bat");
            Console_.WriteLine("Please type (C)ontinue when you install dotnet");
            ConsoleKey lt = ConsoleKey.Z;
            while (lt != ConsoleKey.C)
            {
                lt = Console.ReadKey().Key;
            }
            Console.Write("\r\n");
        }

        public static bool isHaveNET5()
        {
            string[] getRuntimes = tempcmd.Input("dotnet --list-runtimes").Stdout.ToString().Trim().Split();
            bool isWaitingVersion = false;
            for (int i = 0;i < getRuntimes.Length;i++)
            {
                if (isWaitingVersion)
                {
                    if (getRuntimes[i].StartsWith("5.0."))
                    {
                        return true;
                    }
                    else
                    {
                        isWaitingVersion = false;
                        continue;
                    }
                }

                if (getRuntimes[i] == "Microsoft.NETCore.App")
                {
                    isWaitingVersion = true;
                }
            }
            return false;
        }

        public static void InstallNET5()
        {
            if (File.Exists(Path.GetTempPath() + "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"))
            {
                File.Delete(Path.GetTempPath() + "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle");
            }
            File.WriteAllText(Path.GetTempPath() + "InstallDotnet.bat", "winget install -e --id Microsoft.dotnet");
            System.Diagnostics.Process.Start(Path.GetTempPath() + "InstallDotnet.bat");
            Console_.WriteLine("Please type (C)ontinue when you install dotnet");
            ConsoleKey lt = ConsoleKey.Z;
            while (lt != ConsoleKey.C)
            {
                lt = Console.ReadKey().Key;
            }
            Console.Write("\r\n");
        }
    }
}
