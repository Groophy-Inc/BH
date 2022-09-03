using ANSIConsole;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Parser.Utils;
using System.IO;
using BH.Structes;
using BH.ErrorHandle.Error;
using BH.Structes.ErrorStack;

namespace BH.Builder
{
    internal class Build
    {
        public static void Demo(object objwindow)
        {
            ClearTemp(APF.Helper.AssemblyDirectory + "\\Temp");
            Element window = objwindow as Element;
            if (window.Genre.Replace('I', 'i').ToLower() == "window")
            {
                Stopwatch sw = Stopwatch.StartNew();
                CreateFiles(window.Attributes["name"], window.Attributes, window);
                Script.Temp.RunAppByDotnet(true);
                sw.Stop();
                window.Stopwatch = sw;
            }
            else
            {
                Console_.WriteLine("genre is not window, what you try bro??");
                return;
            }
        }

        public static void ClearTemp(string temp)
        {
            if (Directory.Exists(temp))
            {
                Directory.Delete(temp, true);
            }
            Directory.CreateDirectory(temp);
        }
        public static void CreateFiles(string ProjectName, Dictionary<string, string> attr, Element window)
        {
            string tempPath = APF.Helper.AssemblyDirectory + "\\Temp\\";
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            CreateCSPROJ(tempPath, ProjectName,window);
            CreateASSEMBLY(tempPath);
            CreateApp_Xaml(tempPath, ProjectName);
            CreateApp_Xaml_CS(tempPath, ProjectName, window);
            if (!CreateMainWindow_Xaml(tempPath, ProjectName, attr, window))
            {
                Environment.Exit(11);
            }
            CreateMainWindow_Xaml_CS(tempPath, ProjectName, window);
            //CmdFunc.OneTimeInput("dotnet run", CF_Structes.ShellType.ChairmanandManagingDirector_CMD, tempPath);
        }

        private static void CreateMainWindow_Xaml_CS(string tempPath, string ProjectName, Element window)
        {
            string _code = Init(window.appcs);
            string code = JustFixBracket(_code);
            
            File.WriteAllText(tempPath + "MainWindow.xaml.cs", code);
        }

        private static bool CreateMainWindow_Xaml(string tempPath, string ProjectName, Dictionary<string, string> attrs, Element window)
        {
            //            <Window x:Class="CleanTemp.MainWindow"
            //        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            //        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            //        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            //        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
            //        xmlns:local="clr-namespace:CleanTemp"
            //        mc:Ignorable="d"
            //        Title="MainWindow" Height="450" Width="800">
            //    <Grid>

            //    </Grid>
            //</Window>
            string tt = "";
            foreach (var attr in attrs)
            {
                if (attr.Key.ToLower() == "name" || attr.Key.ToLower() == "genre" || attr.Key.ToLower() == "nugets"|| attr.Key.ToLower() == "using")
                {
                    continue;
                }
                if (Builder.Components.Window.isHaveThisKey(attr.Key))
                {
                    tt += attr.Key + "=\"" + attr.Value + "\" ";
                }
                else
                {
                    string getClosest = Builder.Components.Window.getClosest(attr.Key);
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 11,
                        DevCode = 0,
                        ErrorMessage = "Invalid key! this might be what you're looking for: '" + getClosest.Color(ConsoleColor.Green) + "'.".Color(ConsoleColor.Yellow),
                        line = "\""+attr.Key + "\"=\"" + attr.Value + "\"",
                        HighLightLen = 1
                    };

                    ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                    return false;
                }
            }

            string comps = "";

            foreach (var comp in window.comps)
            {
                string adv = "";
                foreach (var keypair in comp.Item3)
                {
                    adv += $"{keypair.Key}=\"{keypair.Value}\" ";
                }

                comps += $"<{comp.Item1} {adv}>{comp.Item2}</{comp.Item1}>\r\n";
            }
            
            File.WriteAllText(tempPath + "MainWindow.xaml", $"<Window x:Class=\"{ProjectName}.MainWindow\"\r\n" +
                            "        xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n" +
                            "        xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n" +
                            "        xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\r\n" +
                            "        xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\r\n" +
                            $"       xmlns:local=\"clr-namespace:{ProjectName}\"\r\n" +
                            "        mc:Ignorable=\"d\"\r\n" +
                            $"        {tt}>\r\n" +  
                            "    <Grid>\r\n" +
                            comps + 
                            "    </Grid>\r\n" +
                            "</Window>\r\n");
            return true;
        }

        private static void CreateCSPROJ(string tempPath, string ProjectName, Element window)
        {
            //<Project Sdk="Microsoft.NET.Sdk">

            //  <PropertyGroup>
            //    <OutputType>WinExe</OutputType>
            //    <TargetFramework>net5.0-windows</TargetFramework>
            //    <Nullable>enable</Nullable>
            //    <UseWPF>true</UseWPF>
            //  </PropertyGroup>

            //</Project>

            string nugs = "";

            foreach (var nuget in window.appcs.Nugets)
            {
                nugs += $"    <PackageReference Include=\"{nuget}\" />\r\n";
            }

            File.WriteAllText(tempPath + $"{ProjectName}.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\">" + @"

  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net5.0-windows</TargetFramework>
    <Nullable>enable</Nullable>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
<ItemGroup>
"+ nugs + @"
  </ItemGroup>
</Project>
");
        }

        private static void CreateApp_Xaml(string tempPath, string ProjectName)
        {
            //            <Application x:Class="CleanTemp.App"
            //             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            //             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            //             xmlns:local="clr-namespace:CleanTemp"
            //             StartupUri="MainWindow.xaml">
            //    <Application.Resources>

            //    </Application.Resources>
            //</Application>

            File.WriteAllText(tempPath + "App.xaml", $"<Application x:Class=\"{ProjectName}.App\"\r\n" +
                "             xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n" +
                "             xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n" +
                $"             xmlns:local=\"clr-namespace:{ProjectName}\"\r\n" +
                "             StartupUri=\"MainWindow.xaml\">\r\n" +
                "    <Application.Resources>\r\n" +
                "    </Application.Resources>\r\n" +
                "</Application>\r\n");
        }

        private static void CreateApp_Xaml_CS(string tempPath, string ProjectName, Element window)
        {
            Structes.BodyClasses._Namespace ns = new Structes.BodyClasses._Namespace()
            {
                //using System;
                //using System.Collections.Generic;
                //            using System.Configuration;
                //            using System.Data;
                //            using System.Linq;
                //            using System.Threading.Tasks;
                //            using System.Windows;
                Using = new List<string>(new string[] { "System.Collections.Generic", "System.Configuration", "System.Data", "System.Linq", "System.Threading.Tasks", "System.Windows" }),
                Name = ProjectName,
                Classes = new List<Structes.BodyClasses._Class>()
                {
                    new Structes.BodyClasses._Class()
                    {
                        Name = "App",
                        isConjunction = true,
                        Conjunction = ": Application",
                        Access = "public partial"
                    }
                }
            };
            string code = JustFixBracket(Init(ns));
            File.WriteAllText(tempPath + "App.xaml.cs", code);
            
        }

        private static void CreateASSEMBLY(string tempPath)
        {
            //            using System.Windows;

            //[assembly: ThemeInfo(
            //    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
            //                                     //(used if a resource is not found in the page,
            //                                     // or application resource dictionaries)
            //    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
            //                                              //(used if a resource is not found in the page,
            //                                              // app, or any theme specific resource dictionaries)
            //)]

            File.WriteAllText(tempPath + "AssemblyInfo.cs", @"using System.Windows;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]
");

        }

        public static Builder.HightLightPack[] hl = new Builder.HightLightPack[]
            {
                new Builder.HightLightPack()
                {
                    KeyWords = new string[]{"class", "string", "namespace", "using"},
                    HexColor = "0096FF"//84dcfa
                },
                new Builder.HightLightPack()
                {
                    KeyWords = new string[]{"public" },
                    HexColor = "5800FF"//1f3065
                }
            };

        public static string Init(Structes.BodyClasses._Namespace _Namespace)
        {
            string Using = @"using System;
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
                if (x.isConjunction) InNamespace += $"{x.Access} class {x.Name} {x.Conjunction}\r\n" + "{\r\n";
                else InNamespace += $"{x.Access} class {x.Name}\r\n" + "{\r\n";

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
{y.Access} {y.ReturnType} {y.Name} {arg} " + "\r\n{" + $@"
{y.Code}
" + "}\r\n";
                    }
                }

                InNamespace += FVoid + "\r\n}";
            }
            InNamespace += "\r\n}";
            return InNamespace;
        }

        public static string JustFixBracket(string text)
        {
            StringBuilder sb = new StringBuilder();
            Random r = new Random();

            int tabcount = 0;


            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c == '{')
                {
                    sb.Append(c.ToString());
                    tabcount++;
                }
                else if (c == '}')
                {
                    sb.Append("[0m"+ c.ToString());
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
                if (index != -1 && startIndex != -1)
                {
                    if (line.Substring(startIndex - 4, 4) == "    ")
                        lines[i] = line.Remove(startIndex - 4, 4);
                }
            }


            return String.Join('\n', lines).ClearANSII();
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
                if (index != -1 && startIndex != -1)
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
                                int Increase = 0;
                                lines[i] = Parser.Utils.Color.ColorByIndex(lines[i], ndx, key.Length, (pack[p].HexColor.StartsWith('#') ? pack[p].HexColor : "#"+pack[p].HexColor), ref Increase).ToString();
                                ndx += Increase;
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
