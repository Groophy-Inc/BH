using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Script
{
    internal class Temp
    {
        public static Dictionary<string, string> HashTemp { get; set; }
        
        public static void ClearTemp() {Array.ForEach(new DirectoryInfo(Path.GetTempPath()).GetFiles("BH_*"), delegate(FileInfo x) { File.Delete(x.FullName); }); LoadHashTemp();}
        public static void LoadHashTemp()
        {
            HashTemp = new Dictionary<string, string>();
            if (File.Exists(Path.GetTempPath() + "BH_HashList.Dictionary"))
            {
                var lines = File.ReadAllLines(Path.GetTempPath() + "BH_HashList.Dictionary");
                foreach (var line in lines)
                {
                    if (line.Contains(','))
                    {
                        string hash = line.Split(',')[0];
                        string FileName = line.Split(',')[1];
                        
                        HashTemp.Add(hash, FileName);
                    }
                }
            }
        }

        public static void SaveHashTemp()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var temp in HashTemp)
            {
                sb.Append(temp.Key + "," + temp.Value+"\r\n");
            }
            File.WriteAllText(Path.GetTempPath()+"BH_HashList.Dictionary", sb.ToString());
        }
        
        public static string HashString(string text, string salt = "KaranveerChouhan")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }
    
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);
        
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }

        public static void RunAppByDotnet(bool printMS = false)
        {
            if (printMS)
            {
                var ot = CmdFunc.OneTimeInput("dotnet build", CF_Structes.ShellType.ChairmanandManagingDirector_CMD, APF.Helper.AssemblyDirectory + "\\Temp\\");
                RunApp();
                ErrorHandle.Logs.Log("Dotnet build Stdout - \r\n" + ot.Stdout.ToString());
            }
            else
            {
                CmdFunc.OneTimeInput("dotnet build", CF_Structes.ShellType.ChairmanandManagingDirector_CMD, APF.Helper.AssemblyDirectory + "\\Temp\\");
                RunApp();
            }
        }

        public static void RunApp()
        {
            string path = APF.Helper.AssemblyDirectory +
                          $"\\Temp\\bin\\Debug\\net5.0-windows\\{Parser.Parse.ProjectName}.exe";
            CmdFunc.OneTimeInput($"call {path}", CF_Structes.ShellType.ChairmanandManagingDirector_CMD, "C:\\", false);
            //System.Diagnostics.Process.Start(path);
        }
    }
}
