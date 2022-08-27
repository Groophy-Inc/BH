using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.APF
{
    internal class Helper
    {
        public static string AssemblyDirectory { get { return Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)); } }

        public static string FixRead(string text)
        {
            string[] lines = text.Split('\n');

            StringBuilder sb = new StringBuilder();
            bool isComment = false;

            foreach (var line in lines)
            {
                if (line.Trim() == "/*") isComment = true;
                else if (line.Trim() == "*/") isComment = false;
                else if (line.StartsWith("//")) continue;
                else
                {
                    if (!isComment) sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }
    }
}
