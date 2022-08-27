using System;
using System.IO;
using BH.ErrorHandle;

namespace BH.APF
{
    public class SaveSystem
    {
        public static string SavePath = "";

        public static void Save()
        {
            if (string.IsNullOrEmpty(SavePath)) return;
            if (!File.Exists(SavePath)) File.Create(SavePath);
            Console_.WriteLine("Logs saving...");
            Logs.Log("Saving...");

            File.WriteAllText("Input.txt", Logs.AllLogs.ToString());

            ClearCurrentConsoleLine(2); Console_.WriteLine("Logs printing to pdf.");

            CmdFunc c = new CmdFunc(APF.Helper.AssemblyDirectory + "/text2pdf", CF_Structes.ShellType.ChairmanandManagingDirector_CMD, false);

            c.Input($"call text2pdf.exe \"{APF.Helper.AssemblyDirectory + "/Input.txt"}\" > \"{SavePath}\"").Print();
            
            ClearCurrentConsoleLine(2); Console_.WriteLine(Logs.AllLogs.Length+" length logs Saved.");

        }
        
        private static void ClearCurrentConsoleLine(int DelLineCount = 0)
        {
            for (int i = 1; i < DelLineCount; i++)
            {
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop - i);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }
            Console.SetCursorPosition(0, Console.CursorTop - DelLineCount + 1);
        }
    }
    
    
}