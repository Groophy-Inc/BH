using System;
using System.IO;
using System.Text;
using ANSIConsole;
using BH.ErrorHandle;
using BH.Parser;

namespace BH.Runner
{
    public class Base
    {
        private static string LastestHash = "NaN";
        
        public static int Run(Func<int> act)
        {
            Initialize.Inıt_All();
            if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;
            if (File.Exists(APF.Helper.AssemblyDirectory + "\\LastestHash.hash")) LastestHash = File.ReadAllText(APF.Helper.AssemblyDirectory + "\\LastestHash.hash");
            if (File.Exists(APF.Helper.AssemblyDirectory + "\\LastestProjectName")) Parse.ProjectName = File.ReadAllText(APF.Helper.AssemblyDirectory + "\\LastestProjectName");
            Console.Title = "BH - ThinkNo - " + Program.Ver;

            int retenv = act();

            if (APF.ArgumentParser.isCheckHashForFastBuild)
            {
                if (LastestHash == HashString(File.ReadAllText(Parse.masterPagePath)))
                {
                    Script.Temp.RunApp();
                }
                else
                {
                    Parser.Parse.ParseMasterPage();
                }
            }
            else Parser.Parse.ParseMasterPage();

            File.WriteAllText(APF.Helper.AssemblyDirectory + "\\LastestHash.hash", HashString(File.ReadAllText(Parse.masterPagePath)));
            File.WriteAllText(APF.Helper.AssemblyDirectory + "\\LastestProjectName", Parse.ProjectName);

            Script.Temp.SaveHashTemp();
            APF.SaveSystem.Save();
            return retenv;
        }
        
        static string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            using var sha = new System.Security.Cryptography.SHA256Managed();
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
            byte[] hashBytes = sha.ComputeHash(textBytes);

            string hash = BitConverter
                .ToString(hashBytes)
                .Replace("-", String.Empty);

            return hash;
        }
    }
}