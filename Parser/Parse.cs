﻿using System;
using System.IO;
using BH.Structes;
using System.Linq;
using ANSIConsole;
using BH.ErrorHandle;
using System.Collections.Generic;

namespace BH.Parser
{
    public class Parse
    {
        public static string srcPath { get; set; }
        public static string masterPagePath { get; set; }
        public static string[] Options { get; set; }

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

        //System
        public static bool System_isPreloading = false; //waits Command
        public static bool System_isWaitingValue = false;
        public static Commands.SystemCommands System_Command = Commands.SystemCommands.NA;
        public static List<string> System_value = new List<string>();

        //UMS dispose ve regen'e ekle
        public static bool UMS_isWaitingHaveVarriableSaveOrNot = false;

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

        public static AllElements ParseMasterPage()
        {
            string[] lines = File.ReadAllLines(masterPagePath);
            Parse.ErrPath = masterPagePath;
            return ParseLines(lines);
        }

        public static AllElements ParseLine(string line)
        {
            Parse.ErrPath = "Custom Input";
            return ParseLines(new string[] { line });
        }

        public static AllElements ParseLines(string[] lines)
        {
            ReGen();
            logErrMsg = "";

            List<Element> allofelements = new List<Element>();

            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i];

                if (string.IsNullOrEmpty(line) || line.StartsWith("//")) continue;

                lineLower = line.Replace("ı", "i").Replace("I", "i").ToLower();
                //For each line start

                LineLen = 0;
                TotalIndexOfLineWords = 0;

                if (Set_isAttributeBuildContent) Set_AttributeBuildContent += "\r\n";
                if (Var_ContentStart) Var_Content += "\r\n";

                string[] words = line.Split(' ');
                for (int j = 0; j < words.Length; j++)
                {
                    word = words[j].Trim();
                    wordLower = word.Replace("ı", "i").Replace("I", "i").ToLower();

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
                        }
                    }


                    if (isProgress && !isSkipThisLine)
                    {
                        //Continue to progress

                        if (ProgressSyntax == "set")
                        {
                            Tuple<bool, Element> retenv = Parser.Commands.Set.Decompose();

                            if (retenv.Item1) allofelements.Add(retenv.Item2);
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

                        }
                        else if (ProgressSyntax == "var")
                        {
                            Parser.Commands.Var.Decompose();
                        }
                    }
                    else
                    {
                        //New progress

                        if (!isSkipThisLine)
                        {
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
                            else
                            {
                                List<Error> errs = new List<Error>();
                                string getClosest = APF.Find_Probabilities.GetClosest(wordLower, Keys.getKeyWordsAsArray());
                                for (int y = 0; y < word.Length; y++)
                                {
                                    Error err = new Error()
                                    {
                                        ErrorPathCode = ErrorPathCodes.Parser,
                                        ErrorID = 1,
                                        DevCode = 0,
                                        ErrorMessage = "Invalid syntax! this might be what you're looking for: '" + getClosest.Color(ConsoleColor.Green) + "'.".Color(ConsoleColor.Yellow),
                                        FilePath = masterPagePath,
                                        line = line,
                                        lineC = LineC,
                                        lenC = LenC + y
                                    };
                                    errs.Add(err);

                                }
                                isSkipThisLine = true;
                                ErrorStack.PrintStack(errs.ToArray());
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
            System_Command = Commands.SystemCommands.NA;
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
            System_Command = Commands.SystemCommands.NA;
            System_value = new List<string>();
        }
    }
}
