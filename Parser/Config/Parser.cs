using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using CSScripting;

namespace BH.Parser.Config
{
    public class Parser
    {
        public static Dictionary<string, Dictionary<string, string>> config { get; private set; } =
            new Dictionary<string, Dictionary<string, string>>();

        public static void Parse()
        {
            string[] lines = File.ReadAllLines(APF.Helper.AssemblyDirectory+"\\config.cfg");
            string lastest_section = "";

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                string line = lines[i];

                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    string section = line.TrimStart('[').TrimEnd(']');
                    try
                    {
                        var ishave = config[section];
                    }
                    catch
                    {
                        config.Add(section, new Dictionary<string, string>());
                    }

                    lastest_section = section;
                }
                else
                {
                    try
                    {
                        string key = line.Split('=')[0];
                        string value =string.Join('=',  line.Split('=').Skip(1).ToArray());

                        var x = config[lastest_section];
                        x.Add(key, value);
                    }
                    catch { }
                }
            }
            return;
        }

        public class Config
        {
            public static string Read(string key, string section)
            {
                try
                {
                    return config[section][key];
                }
                catch (Exception e)
                {
                    return "Null";
                }
            }
        }
    }
}