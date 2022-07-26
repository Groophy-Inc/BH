﻿using System;
using System.IO;
using BH.Structes;
using System.Linq;
using ANSIConsole;
using BH.ErrorHandle;
using System.Collections.Generic;
using System.ComponentModel;
using BH.ErrorHandle.Error;
using BH.Parser.Commands;
using BH.Structes.ErrorStack;

namespace BH.Parser
{
    public class Parse
    {
        public static string srcPath { get; set; }
        public static string masterPagePath { get; set; }

        //General
        public static bool isProgress = false;
        public static string ProgressSyntax = "";
        public static string ErrPath = "";
        public static int LineLen = 0;
        public static bool isSkipSecondCheck = false;
        public static bool isSkipThisLine = false; //syntax error or comment flag
        public static string line = "";
        public static string lineLower = "";
        public static string word = "";
        public static string wordLower = "";
        public static int LineC = 0;
        public static int LenC = 0;
        public static int wordNumber = 0;
        public static bool isBackslashableContent = false;
        public static int TotalIndexOfLineWords = 0;
        public static string logErrMsg = "";
        public static bool isAnyContent = false;
        public static bool isCommentBlock = false;
        public static string ProjectName = "";
        public static List<CommandBuilder> Commands = new List<CommandBuilder>();
        public static pbp_Command callit = new pbp_Command();
        public static bool isNewLine = false;

        //General Handle
        public static bool isErrorStack = false;
        public static System.Collections.Generic.List<Error> errors = new System.Collections.Generic.List<Error>();
        public static int StackErrorID = -1;

        //Set
        public static bool Set_isWaitingName = false;
        public static bool Set_isWaitingGenre = false;
        public static bool Set_isWaitingAttributes = false;
        public static bool Set_isAttributeBuild = false;
        public static bool Set_isAttributeBuildName = false;
        public static string Set_AttributeBuildName = "";
        public static bool Set_isAttributeBuildContentStart = false;
        public static bool Set_isAttributeBackSlash = false;
        public static bool Set_isAttributeBuildContent = false;
        public static string Set_AttributeBuildContent = "";
        public static bool Set_isNewAttributeWaiting = false;
        public static System.Collections.Generic.Dictionary<string, string> Set_Attributes = new System.Collections.Generic.Dictionary<string, string>();

        //Msg
        public static bool Msg_isWaitingContentStart = false;
        public static bool Msg_isContentStart = false;
        public static bool Msg_isBackSlash = false;
        public static string Msg_Content = "";
        public static bool Msg_ContentWithoutConKey = false;

        //System
        public static bool System_isPreloading = false; //waits Command
        public static bool System_isWaitingValue = false;
        public static Commands.SystemCommands System_Command = SystemCommands.NA;
        public static List<string> System_value = new List<string>();

        //UMS dispose ve regen'e ekle
        public static bool UMS_isWaitingHaveVarriableSaveOrNot = false;
        public static bool UMS_isSaveAsVarriable = false;
        public static string UMS_SaveAsVarriableName = "";
        public static bool UMS_isWaitingAS = false;
        public static bool UMS_isWaitingLang = false;
        public static Commands.IScriptTypes UMS_Lang = IScriptTypes.Null;
        public static bool UMS_isWaitingConKey = false;
        public static bool UMS_isWaitingContentKey = false;
        public static bool UMS_ContentStart = false;
        public static bool UMS_isAttributeBackSlash = false;
        public static bool UMS_isWaitingSemiColon = false;
        public static string UMS_Content = "";

        //Var
        public static bool Var_isWaitingVarName = false;
        public static string Var_VarName = "";
        public static bool Var_isWaitingConKey = false;
        public static bool Var_isWaitingASorContentStart = false;
        public static bool Var_isAS = false;
        public static string Var_ASLang = "";
        public static bool Var_ContentStart = false;
        public static string Var_Content = "";
        public static bool Var_isAttributeBackSlash = false;
        public static bool Var_isWaitingSemiColon = false;

        //Ankita
        public static bool Ankita_isWaitingDEBUG = false;
        public static bool Ankita_isWaitingBoolean = false;
        public static bool Ankita_DEBUGBoolean = false;
        public static bool Ankita_isWaitingEndKey = false;

        //Version
        public static bool Version_isWaitingEndKey = false;

        //Show
        public static bool Show_isWaitingVarriable = false;

        public static AllElements ParseMasterPage()
        {
            string[] lines = File.ReadAllLines(masterPagePath);
            Parse.ErrPath = masterPagePath;
            return _ParseLines(lines);
        }

        public static AllElements ParseLine(string line)
        {
            Parse.ErrPath = "Custom Input";
            return ParseLines(new string[] { line });
        }

        public static AllElements ParseLines(string[] lines)
        {
            Parse.ErrPath = "Custom Input";
            return _ParseLines(lines);
        }

        public static void EndProcess()
        {
            Parse.isProgress = false;
            Parse.ProgressSyntax = "";
            Parse.isBackslashableContent = false;
            Logs.isTab = false;
        }

        private static AllElements _ParseLines(string[] lines)
        {
            Logs.Log("--------------------------------------------------------\r\nParse started.\r\n--------------------------------------------------------\r\n");
            Logs.Log(string.Join("\r\n", lines));
            Logs.Log("--------------------------------------------------------");
            ReGen();
            logErrMsg = "";

            List<Element> allofelements = new List<Element>();

            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i];

                isNewLine = true;

                if (!isAnyContent) line += " ";

                if ((string.IsNullOrEmpty(line) || line.StartsWith("//")) && !isAnyContent) continue;

                if (line.Trim().StartsWith("/*") && !isAnyContent) isCommentBlock = true;

                if (isCommentBlock)
                {
                    if (line.Trim().EndsWith("*/")) isCommentBlock = false;

                    continue;
                }

                lineLower = line.Replace("ı", "i").Replace("I", "i").ToLower();
                //For each line start

                Logs.Log("\r\nNew Line: '" + line + "'");

                LineLen = 0;
                TotalIndexOfLineWords = 0;

                if (Set_isAttributeBuildContent) Set_AttributeBuildContent += "\r\n";
                if (Var_ContentStart) Var_Content += "\r\n";
                if (UMS_ContentStart) UMS_Content += "\r\n";

                string[] words = line.Split(' ');
                for (int j = 0; j < words.Length; j++)
                {
                    if (j != 0) isNewLine = false;
                    
                    word = words[j].Trim();
                    wordLower = word.Replace("ı", "i").Replace("I", "i").ToLower();

                    if (!isAnyContent)
                    {
                        if (string.IsNullOrEmpty(word)) { TotalIndexOfLineWords += word.Length + 1; continue; }
                    }

                    Logs.Log($"New word: '{word}'");

                    LineC = i;
                    LenC = LineLen + j;
                    wordNumber = j;

                    //For each word of line start

                    if (!isBackslashableContent)
                    {
                        //Comment
                        if (wordLower.StartsWith("//"))
                        {
                            isSkipThisLine = true;
                            Logs.Log("Comment line.");
                        }
                    }


                    if (isProgress && !isSkipThisLine)
                    {
                        //Continue to progress

                        if (ProgressSyntax == "set")
                        {
                            Tuple<bool, Element> retenv = Parser.Commands.Set.Decompose();

                            if (retenv.Item1)
                            {
                                if (retenv.Item2.Genre == "window")
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
                                        Using = new System.Collections.Generic.List<string>(new string[] { "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks", "System.Windows", "System.Windows.Controls", "System.Windows.Data",
                                            "System.Windows.Documents","System.Windows.Input","System.Windows.Media","System.Windows.Media.Imaging","System.Windows.Navigation","System.Windows.Shapes", "BH"}),
                                        Name = ProjectName,
                                        Classes = new List<Structes.BodyClasses._Class>()
                                        {
                                            new Structes.BodyClasses._Class()
                                            {
                                                Name = "MainWindow",
                                                isConjunction = true,
                                                Conjunction = ": Window",
                                                Access = "public partial",
                                                Voides = new List<Structes.BodyClasses._Void>()
                                                {
                                                    new Structes.BodyClasses._Void()
                                                    {
                                                        Access="public",
                                                        Args=new List<string>(),
                                                        Code = "InitializeComponent();",
                                                        Name = "MainWindow",
                                                        ReturnType = ""
                                                    }
                                                }
                                            }
                                        }
                                    };
                                    string _ = "NaN";
                                    if (retenv.Item2.Attributes.TryGetValue("nugets", out _))
                                    {
                                        foreach (var nuget in _.Split(','))
                                        {
                                            ns.Nugets.Add(nuget);
                                        }
                                    }
                                    
                                    _ = "NaN";
                                    if (retenv.Item2.Attributes.TryGetValue("using", out _))
                                    {
                                        foreach (var _using in _.Split(','))
                                        {
                                            ns.Using.Add(_using);
                                        }
                                    }
                                    retenv.Item2.appcs = ns;
                                }
                                
                                allofelements.Add(retenv.Item2);
                                Varriables.AddorUpdate(retenv.Item2.Attributes["name"], retenv.Item2);
                            }
                        }
                        else if (ProgressSyntax == "msg")
                        {
                            Parser.Commands.Msg.Decompose();
                        }
                        else if (ProgressSyntax == "system")
                        {
                            Parser.Commands.system.Decompose();
                        }
                        else if (ProgressSyntax == "ums")
                        {
                            Parser.Commands.ums.Decompose();
                        }
                        else if (ProgressSyntax == "var")
                        {
                            Parser.Commands.Var.Decompose();
                        }
                        else if (ProgressSyntax == "ankita")
                        {
                            Parser.Commands.Ankita.Decompose();
                        }
                        else if (ProgressSyntax == "version")
                        {
                            Parser.Commands.version.Decompose();
                        }
                        else if (ProgressSyntax == "show")
                        {
                            Parser.Commands.Show.Decompose();
                        }

                        bool isHaveInCommands = Commands.Any(x => x.Registery.CommandName == ProgressSyntax);
                        if (isHaveInCommands)
                        {
                            var com = Commands.Where(x => x.Registery.CommandName == ProgressSyntax).First();

                            
                            com.Registery.Delegate(callit);
                        }

                    }
                    else
                    {
                        //New progress

                        if (!isSkipThisLine)
                        {
                            bool isAccept = true;
                            if (wordLower == "set")
                            {
                                isProgress = true;
                                ProgressSyntax = "set";
                                Set_isWaitingName = true;
                            }
                            else if (wordLower == "msg")
                            {
                                isProgress = true;
                                ProgressSyntax = "msg";
                                Msg_isWaitingContentStart = true;
                            }
                            else if (wordLower == "system")
                            {
                                isProgress = true;
                                ProgressSyntax = "system";
                                System_isPreloading = true;
                            }
                            else if (wordLower == "ums") //UnManagement Script
                            {
                                isProgress = true;
                                ProgressSyntax = "ums";
                                UMS_isWaitingHaveVarriableSaveOrNot = true;
                            }
                            else if (wordLower == "var") 
                            {
                                isProgress = true;
                                ProgressSyntax = "var";
                                Var_isWaitingVarName = true;
                               
                            }
                            else if (wordLower == "ankita")
                            {
                                isProgress = true;
                                ProgressSyntax = "ankita";
                                Ankita_isWaitingDEBUG = true;
                            }
                            else if (wordLower == "version;")
                            {
                                Console_.WriteLine(APF.Version.Read);
                                Logs.Log($"Syntax: '{ProgressSyntax}' [{DateTime.Now.ToString("HH:mm:ss")}]\r\n");
                                Logs.isTab = true;

                                Logs.OnPropertyChanged("isProgress", "version");
                                Logs.OnPropertyChanged("ProgressSyntax", false);
                                Logs.OnPropertyChanged("Version_isWaitingEndKey", true);

                                Logs.Log("Version printed.");
                                Logs.Log("Process end;");

                                Logs.isTab = false;
                                isAccept = false;
                            }
                            else if (wordLower == "version")
                            {
                                isProgress = true;
                                ProgressSyntax = "version";
                                Version_isWaitingEndKey = true;
                                Logs.OnPropertyChanged("isProgress", "version");
                                Logs.OnPropertyChanged("ProgressSyntax", false);
                                Logs.OnPropertyChanged("Version_isWaitingEndKey", true);
                            }
                            else if (wordLower == "show")
                            {
                                isProgress = true;
                                ProgressSyntax = "show";
                                Show_isWaitingVarriable = true;
                                Logs.OnPropertyChanged("isProgress", "show");
                                Logs.OnPropertyChanged("ProgressSyntax", false);
                                Logs.OnPropertyChanged("Show_isWaitingVarriable", true);
                            }
                            else
                            {
                                bool isHaveInCommands = Commands.Any(x => x.Registery.CommandName == wordLower);
                                if (isHaveInCommands)
                                {
                                    isProgress = true;
                                    ProgressSyntax = wordLower;
                                    var com = Commands.Where(x => x.Registery.CommandName == wordLower).FirstOrDefault();
                                    callit = new pbp_Command();
                                    callit.CommandName = com.Registery.CommandName;
                                    callit.Commands = new List<Command>();
                                    foreach (var _command in com.Registery.Commands)
                                    {
                                        Command c = new Command();
                                        c.Type = _command.Type;
                                        c.Value = _command.Value;
                                        c.StatusValue_NotForUsers = _command.StatusValue_NotForUsers;
                                        callit.Commands.Add(c);
                                    }
                                }
                                else
                                {
                                    isAccept = false;
                                    string getClosest = APF.Find_Probabilities.GetClosest(wordLower, Keys.getKeyWordsAsArray());
                                    Error err = new Error()
                                    {
                                        ErrorPathCode = ErrorPathCodes.Parser,
                                        ErrorID = 1,
                                        DevCode = 0,
                                        ErrorMessage = "Invalid syntax! this might be what you're looking for: '" + getClosest.Color(Config.Parser.Config.Read("Suggestion","Error")) + "'.".Color(ConsoleColor.Yellow),
                                        line = line,
                                        HighLightLen = word.Length
                                    };
                                    isSkipThisLine = true;
                                    ErrorStack.PrintStack(err,new System.Diagnostics.StackFrame(0, true));
                                }
                            }

                            if (isAccept) //if syntax accepted
                            {
                                Logs.Log($"Syntax: '{ProgressSyntax}' [{DateTime.Now.ToString("HH:mm:ss")}]\r\n");
                                Logs.isTab = true;
                            }
                        }
                    }

                    TotalIndexOfLineWords += word.Length + 1;

                    //For each word of line end
                    if (isSkipThisLine)
                    {
                        isSkipThisLine = false;
                        break;
                    }
                }

                //For each line end


            }

            Dispose();

            return new AllElements() { ElementList = allofelements.ToArray() };
        }

        public static void Dispose()
        {
            isProgress = false;
            ProgressSyntax = null;
            LineLen = 0;
            isSkipSecondCheck = false;
            isSkipThisLine = false; 
            line = null;
            lineLower = null;
            word = null;
            wordLower = null;
            LineC = 0;
            LenC = 0;
            wordNumber = 0;
            isBackslashableContent = false;
            TotalIndexOfLineWords = 0;
            logErrMsg = null;
            isErrorStack = false;
            errors = null;
            StackErrorID = 0;
            Set_isWaitingName = false;
            Set_isWaitingGenre = false;
            Set_isWaitingAttributes = false;
            Set_isAttributeBuild = false;
            Set_isAttributeBuildName = false;
            Set_AttributeBuildName = null;
            Set_isAttributeBuildContentStart = false;
            Set_isAttributeBackSlash = false;
            Set_isAttributeBuildContent = false;
            Set_AttributeBuildContent = null;
            Set_isNewAttributeWaiting = false;
            Set_Attributes = null;
            Msg_isWaitingContentStart = false;
            Msg_isContentStart = false;
            Msg_isBackSlash = false;
            Msg_Content = null;
            System_isPreloading = false;
            System_isWaitingValue = false;
            System_Command = SystemCommands.NA;
            System_value = null;
        }

        public static void ReGen()
        {
            isProgress = false;
            ProgressSyntax = "";
            LineLen = 0;
            isSkipSecondCheck = false;
            isSkipThisLine = false; 
            line = "";
            lineLower = "";
            word = "";
            wordLower = "";
            LineC = 0;
            LenC = 0;
            wordNumber = 0;
            isBackslashableContent = false;
            TotalIndexOfLineWords = 0;
            logErrMsg = "";
            isErrorStack = false;
            errors = new System.Collections.Generic.List<Error>();
            StackErrorID = -1;
            Set_isWaitingName = false;
            Set_isWaitingGenre = false;
            Set_isWaitingAttributes = false;
            Set_isAttributeBuild = false;
            Set_isAttributeBuildName = false;
            Set_AttributeBuildName = "";
            Set_isAttributeBuildContentStart = false;
            Set_isAttributeBackSlash = false;
            Set_isAttributeBuildContent = false;
            Set_AttributeBuildContent = "";
            Set_isNewAttributeWaiting = false;
            Set_Attributes = new System.Collections.Generic.Dictionary<string, string>();
            Msg_isWaitingContentStart = false;
            Msg_isContentStart = false;
            Msg_isBackSlash = false;
            Msg_Content = "";
            System_isPreloading = false;
            System_isWaitingValue = false;
            System_Command = SystemCommands.NA;
            System_value = new List<string>();
        }
    }
}
