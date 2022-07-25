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

        public static AllElements ParseMasterPage(string _srcPath, string _masterPagePath, string[] _Options)
        {
            
            srcPath = _srcPath;
            masterPagePath = _masterPagePath;
            Options = _Options;

            //GetElements.Get = JsonConvert.DeserializeObject<AllElements>(File.ReadAllText(masterPagePath)).ElementList;

            string[] lines = File.ReadAllLines(masterPagePath);

            bool isProgress = false;
            string ProgressSyntax = "";
            bool isWaitingName = false;
            bool isWaitingGenre = false;
            bool isWaitingAttributes = false;
            bool isAttributeBuild = false;
            bool isAttributeBuildName = false;
            string AttributeBuildName = "";
            bool isAttributeBuildContentStart = false;
            bool isAttributeBackSlash = false;
            bool isAttributeBuildContent = false;
            string AttributeBuildContent = "";
            bool isNewAttributeWaiting = false;
            var Attributes = new System.Collections.Generic.Dictionary<string, string>();
            int LineLen = 0;
            bool isSkipSecondCheck = false;
            bool isSkipThisLine = false; //syntax error or comment flag

            bool isErrorStack = false;
            var errors = new System.Collections.Generic.List<Error>();
            int StackErrorID = -1;

            List<Element> allofelements = new List<Element>();
            
            /*
            
            , veya ; kısımlarında bu özellikleri sıfırla

             */

            for (int i = 0;i < lines.Length;i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrEmpty(line)) continue;

                string lineLower = line.Replace("ı", "i").Replace("I", "i").ToLower();
                //For each line start

                LineLen = 0;

                if (isAttributeBuildContent)
                {
                    AttributeBuildContent += "\r\n";
                }

                string[] words = line.Split(' ');
                for (int j = 0; j < words.Length; j++)
                {
                    string word = words[j].Trim();
                    string wordLower = word.Replace("ı", "i").Replace("I", "i").ToLower();

                    int LineC = i;
                    int LenC = LineLen + j;

                    //For each word of line start

                    if (isAttributeBuildContent && j != 0)
                    {
                        AttributeBuildContent += " ";
                    }

                    if (!isAttributeBuild)
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
                        
                        if (isWaitingName)
                        {
                            string __name = word;
                            if (word[0] == '$') __name = __name.Substring(1);
                            Attributes.Add("name", __name);
                            isWaitingName = false;
                            isWaitingGenre = true;
                            LogSystem.log("New Attribute - '" + "name" + "': " + __name, ConsoleColor.Green);
                        }
                        else if (isWaitingGenre)
                        {
                            string __genre = wordLower.TrimEnd(':').Trim().TrimEnd(')').TrimStart('(');
                            Attributes.Add("genre", __genre);
                            isWaitingGenre = false;
                            isWaitingAttributes = true;
                            LogSystem.log("New Attribute - '" + "genre" + "': " + __genre, ConsoleColor.Green);
                        }
                        else if (isWaitingAttributes)
                        {
                            for (int p =0;p <word.Length;p++)
                            {
                                //For each char of word
                                char _c = ' ';
                                char c = word[p];
                                char c_ = ' '; //second char

                                bool isError = false;
                                Error error = new Error();

                                
                                

                                if (p + 1 < word.Length) c_ = word[p + 1];
                                if (p - 1 < word.Length && p-1>=0) _c = word[p - 1];

                                if (isAttributeBuild)
                                {
                                    if (isAttributeBuildName)
                                    {
                                        //key
                                        if (c == '"')
                                        {
                                            isAttributeBuildName = false;
                                            
                                        }
                                        else
                                        {
                                            AttributeBuildName += c;
                                        }
                                    }
                                    else
                                    {
                                        if (c == ':')
                                        {
                                            isAttributeBuildContent = true;
                                            isSkipSecondCheck = true;
                                        }
                                        else
                                        {
                                            if (isAttributeBuildName)
                                            {

                                                Error err = new Error()
                                                {
                                                    ErrorPathCode = ErrorPathCodes.Parser,
                                                    ErrorID = 0,
                                                    DevCode = 2,
                                                    ErrorMessage = "Spilled out obscure object",
                                                    FilePath = masterPagePath,
                                                    line = line,
                                                    lineC = LineC,
                                                    lenC = LenC
                                                };
                                                isError = true;
                                                error = err;
                                            }
                                        }
                                        
                                        //value
                                        if (isSkipSecondCheck)
                                        {
                                            isSkipSecondCheck = false;
                                        }
                                        else
                                        {
                                            if (isAttributeBuildContentStart)
                                            {
                                                if (c == '\\')
                                                {
                                                    isAttributeBackSlash = true;
                                                }
                                                else
                                                {
                                                    if (isAttributeBackSlash)
                                                    {
                                                        AttributeBuildContent += c;
                                                        isAttributeBackSlash = false;
                                                    }
                                                    else
                                                    {
                                                        if (c == '"')
                                                        {
                                                            isAttributeBuildContentStart = false;
                                                            isAttributeBuildContent = false;
                                                            isAttributeBuild = false;
                                                            Attributes.Add(AttributeBuildName, AttributeBuildContent);
                                                            LogSystem.log("New Attribute - '" + AttributeBuildName + "': " + AttributeBuildContent, ConsoleColor.Green);
                                                            AttributeBuildName = "";
                                                            AttributeBuildContent = "";
                                                            isNewAttributeWaiting = true;
                                                        }
                                                        else
                                                        {
                                                            AttributeBuildContent += c;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (c == '"')
                                                {
                                                    if (isAttributeBuildContent)
                                                    {
                                                        isAttributeBuildContentStart = true;
                                                    }
                                                }
                                                else
                                                {
                                                    Error err = new Error()
                                                    {
                                                        ErrorPathCode = ErrorPathCodes.Parser,
                                                        ErrorID = 0,
                                                        DevCode = 0,
                                                        ErrorMessage = "Spilled out obscure object",
                                                        FilePath = masterPagePath,
                                                        line = line,
                                                        lineC = LineC,
                                                        lenC = LenC
                                                    };
                                                    isError = true;
                                                    error = err;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //new attribute or end
                                    if (c == '"')
                                    {
                                        isAttributeBuild = true;
                                        isAttributeBuildName = true;
                                    }
                                    else if (c == ',')
                                    {
                                        isAttributeBuild = false;
                                    }
                                    else if (c == ';')
                                    {
                                        allofelements.Add(new Element()
                                        {
                                            Genre = Attributes["genre"],
                                            Attributes = Attributes
                                        });

                                        LogSystem.log("New element added to element list name of $" + Attributes["name"].Color(ConsoleColor.Magenta), ConsoleColor.DarkGreen);

                                        isProgress = false;
                                        ProgressSyntax = "";
                                        isWaitingName = false;
                                        isWaitingGenre = false;
                                        isWaitingAttributes = false;
                                        isAttributeBuild = false;
                                        isAttributeBuildName = false;
                                        AttributeBuildName = "";
                                        isAttributeBuildContentStart = false;
                                        isAttributeBackSlash = false;
                                        isAttributeBuildContent = false;
                                        AttributeBuildContent = "";
                                        isNewAttributeWaiting = false;
                                        Attributes = new System.Collections.Generic.Dictionary<string, string>();
                                    }
                                    else
                                    {
                                        Error err = new Error()
                                        {
                                            ErrorPathCode = ErrorPathCodes.Parser,
                                            ErrorID = 0,
                                            DevCode = 1,
                                            ErrorMessage = "Spilled out obscure object",
                                            FilePath = masterPagePath,
                                            line = line,
                                            lineC = LineC,
                                            lenC = LenC
                                        };
                                        isError = true;
                                        error = err;
                                    }
                                }

                                //Check error stack
                                if (isErrorStack)
                                {
                                    if (isError)
                                    {
                                        if (error.ErrorID == StackErrorID)
                                        {
                                            errors.Add(error);
                                        }
                                        else //new error stack
                                        {
                                            ErrorStack.PrintStack(errors.ToArray());
                                            isErrorStack = true;
                                            errors = new System.Collections.Generic.List<Error>();
                                            StackErrorID = error.ErrorID;
                                        }
                                    }
                                    else
                                    {
                                        ErrorStack.PrintStack(errors.ToArray());
                                        isErrorStack = false;
                                        errors = new System.Collections.Generic.List<Error>();
                                        StackErrorID = -1;
                                    }
                                }
                                else
                                {
                                    if (isError)
                                    {
                                        isErrorStack = true;
                                        StackErrorID = error.ErrorID;
                                        errors.Add(error);
                                    }
                                }

                                LineLen++;
                            }
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
                                isWaitingName = true;
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
