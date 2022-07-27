using Newtonsoft.Json;
using System;
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

        

        public static AllElements ParseMasterPage(string _srcPath, string _masterPagePath, string[] _Options)
        {
            
            srcPath = _srcPath;
            masterPagePath = _masterPagePath;
            Options = _Options;

            string[] lines = File.ReadAllLines(masterPagePath);

            List<Element> allofelements = new List<Element>();

            for (int i = 0;i < lines.Length;i++)
            {
                line = lines[i].Trim();

                if (string.IsNullOrEmpty(line)) continue;

                lineLower = line.Replace("ı", "i").Replace("I", "i").ToLower();
                //For each line start

                LineLen = 0;
                TotalIndexOfLineWords = 0;

                if (Set_isAttributeBuildContent)
                {
                    Set_AttributeBuildContent += "\r\n";
                }

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

            return new AllElements() { ElementList = allofelements.ToArray() };
        }
    }
}
