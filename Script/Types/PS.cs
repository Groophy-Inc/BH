using System;
using System.IO;
using System.Linq;
using System.Text;

namespace BH.Script.Types
{
    public class PS
    {
        public static BH.CmdFunc Terminal = new CmdFunc(System.IO.Path.GetTempPath(),
            CF_Structes.ShellType.PowerShell_PS, false);
        
        public static object Execute(string script, int timeoutMS = 3000)
        {
            string hash = Temp.HashString(script);
            bool BeforeWrited = Temp.HashTemp.Any(x => x.Key == hash);
            if (BeforeWrited)
            {
                string fileName = Temp.HashTemp[hash];
                var retenv = Terminal.Input(".\\" + "BH_" + fileName + ".ps1", timeoutMS);
                retenv.Stdin = script;
                return retenv;
            }
            else
            {
                string guid = GenWord(40);
                while (File.Exists(Path.GetTempPath() + "BH_" + guid + ".ps1"))
                {
                    guid = GenWord(25);
                }

                var fileName = Path.GetTempPath() + "BH_" + guid + ".ps1";

                File.WriteAllText(fileName, script);

                var retenv = Terminal.Input(".\\" + "BH_" + guid + ".ps1", timeoutMS);

                retenv.Stdin = script;
                Temp.HashTemp.Add(hash, ".\\" + "BH_" + guid + ".ps1");

                return retenv;
            }
        }

        public static object GetFieldOfObject(object Key, string fieldName) => 
            (Key.GetType().GetFields().Any(x => x.Name == fieldName)) 
                ? Key.GetType().GetField(fieldName).GetValue(Key) 
                : new object();

        static string GenWord(int len)
        {
            StringBuilder sb = new StringBuilder(len);
            Random r = new Random();
            for (int i = 0; i < len; i++)
            {
                sb.Append((char)(r.Next(65, 90)));
            }

            return sb.ToString();
        }
    }
}