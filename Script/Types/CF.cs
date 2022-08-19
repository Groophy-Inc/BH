using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BH.Script.Types
{
    public class CF
    {
        public static BH.CmdFunc Terminal = new CmdFunc(System.IO.Path.GetTempPath(),
            CF_Structes.ShellType.ChairmanandManagingDirector_CMD, false);
        
        public static object Execute(string script, int timeoutMS = 3000)
        {
            string guid = GenWord(25);
            while (File.Exists(Path.GetTempPath() + guid + ".bat"))
            {
                guid = GenWord(25);
            }

            var fileName = Path.GetTempPath() + guid + ".bat";
            
            File.WriteAllText(fileName, script);

            return Terminal.Input("call \"" + guid + ".bat\"", timeoutMS);
        }

        public static object GetFieldOfObject(object Key, string fieldName) => 
            (Key.GetType().GetFields().Any(x => x.Name == fieldName)) 
                ? Key.GetType().GetField(fieldName).GetValue(Key) 
                : new object();

        /*public static object GetFieldOfObject(object Key, string fieldName)
        {
            bool ok = Key.GetType().GetFields().Any(x => x.Name == fieldName);
            if (ok)
            {
                var ret = Key.GetType().GetField(fieldName).GetValue(Key);
                return ret;
            }

            return new object();
        }*/

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