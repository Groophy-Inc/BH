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
            Console_.WriteLine("Logs saving...");
            Logs.Log("Saving...");

            File.WriteAllText("Input.txt", Logs.AllLogs.ToString());

            ClearCurrentConsoleLine(2); Console.WriteLine("Logs printing to pdf.");

            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "text2pdf/text2pdf.exe";
            p.StartInfo.Arguments = $"\"{APF.Helper.AssemblyDirectory}\\Input.txt\" \"{SavePath}\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();

            ClearCurrentConsoleLine(2); Console.WriteLine(Logs.AllLogs.Length+" length logs Saved.");

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