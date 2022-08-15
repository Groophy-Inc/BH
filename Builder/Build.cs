using ANSIConsole;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Parser.Utils;

namespace BH.Builder
{
    internal class Build
    {
        public static string Init(Structes.BodyClasses._Namespace _Namespace)
        {
            string Using = @"
using System;
"; //using

            foreach (var x in _Namespace.Using)
            {
                Using += "using " + x+";\r\n";
            }

            string Namespace = @$"namespace {_Namespace.Name}"+"\r\n" + "{\r\n"; //namespace with {
            string InNamespace = ""; //in Namespace
            string FClass = ""; //foreach class
            string FVoid = ""; //foreach void

            InNamespace += Using + "\r\n";

            InNamespace += Namespace;

            _Namespace.Classes.Sort();
            foreach (var x in _Namespace.Classes)
            {
                InNamespace += $"public class {x.Name}\r\n" + "{\r\n";

                foreach (var y in x.Voides)
                {
                    if (y.isField)
                    {
                        FVoid += $@"{y.Access} {y.ReturnType} {y.Name} {((y.FieldDefualt.EndsWith(';') || y.FieldDefualt.EndsWith('}')) ? y.FieldDefualt : y.FieldDefualt + ";")}";
                    }
                    else
                    {
                        string arg = "(";
                        for (int i = 0; i < y.Args.Count(); i++)
                        {
                            arg += "object " + y.Args[i] + ",";
                        }
                        if (!arg.Equals("(")) arg = arg.Substring(0, arg.Length - 1);
                        arg += ")";
                        FVoid += @$"
{y.Access} {y.Name} {arg} " + "\r\n{" + $@"
{y.Code}
" + "}\r\n";
                    }
                }

                InNamespace += FVoid + "\r\n}";
            }
            InNamespace += "\r\n}";
            return InNamespace;
        }

        public static string HighLightBracket(string text, HightLightPack[] pack)
        {
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            List<System.Drawing.Color> randomColors = new List<System.Drawing.Color>();

            int opencount = -1;
            int tabcount = 0;


            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c == '{')
                {
                    randomColors.Add(System.Drawing.Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0));
                    opencount++;
                    sb.Append(c.ToString().Color(randomColors[opencount]));
                    tabcount++;
                }
                else if (c == '}')
                {
                    sb.Append(c.ToString().Color(randomColors[opencount]));
                    randomColors.RemoveAt(opencount);
                    opencount--;
                    tabcount--;
                }
                else
                {
                    sb.Append(c);
                    if (c == '\n') sb.Append(new String(' ', tabcount * 4)); 
                    
                }
            }

            string[] lines = sb.ToString().Split('\n');
            sb.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int startIndex = line.IndexOf('');
                int index = line.IndexOf("}");
                if (index != -1)
                {
                    if (line.Substring(startIndex - 4, 4) == "    ")
                        lines[i] = line.Remove(startIndex - 4, 4);
                }

                string[] parts = line.Split(' ');
                int ndx = 0;
                for (int j = 0;j < parts.Length; j++)
                {
                    string word = parts[j].Replace('I', 'i').ToLower();
                    for (int p = 0; p < pack.Length; p++)
                    {
                        for (int g = 0; g< pack[p].KeyWords.Length;g++)
                        {
                            string key = pack[p].KeyWords[g];
                            if (word == key)
                            {
                                if (word == "public" )
                                {
                                    string a = "";
                                }
                                lines[i] = Parser.Utils.Color.ColorByIndex(lines[i], ndx, key.Length, (pack[p].HexColor.StartsWith('#') ? pack[p].HexColor : "#"+pack[p].HexColor)).ToString();
                            }
                        }
                    }
                    ndx += word.Length + 1;
                }
            }


            return String.Join('\n', lines);
        }

    }

    public class HightLightPack
    {
        public string[] KeyWords { get; set; }
        public string HexColor { get; set; }
    }
}
