using Newtonsoft.Json;
using System;
using System.IO;
using BH.Structes;
using System.Linq;
using ANSIConsole;
using BH.ErrorHandle;

namespace BH.Parser
{
    public class Parse
    {
        public static string srcPath { get; set; }
        public static string masterPagePath { get; set; }
        public static string[] Options { get; set; }

        public static void ParseMasterPage(string _srcPath, string _masterPagePath, string[] _Options)
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
            bool isAttributeBuildContent = false;
            string AttributeBuildContent = "";
            var Attributes = new System.Collections.Generic.Dictionary<string, string>();
            int LineLen = 0;
            bool isSkipSecondCheck = false;

            bool isErrorStack = false;
            var errors = new System.Collections.Generic.List<Error>();
            int StackErrorID = -1;
            
            /*
            
            , veya ; kısımlarında bu özellikleri sıfırla

             */

            for (int i = 0;i < lines.Length;i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrEmpty(line)) continue;

                string lineLower = line.Replace("ı", "i").Replace("I", "i").ToLower();
                //For each line start

                string[] words = line.Split(' ');
                for (int j = 0; j < words.Length; j++)
                {
                    string word = words[j].Trim();
                    string wordLower = word.Replace("ı", "i").Replace("I", "i").ToLower();
                    //For each word of line start

                    if (isProgress)
                    {
                        //Continue to progress
                        
                        if (isWaitingName)
                        {
                            string __name = word;
                            if (word[0] == '$') __name = __name.Substring(1);
                            Attributes.Add("name", __name);
                            isWaitingName = false;
                            isWaitingGenre = true;
                        }
                        else if (isWaitingGenre)
                        {
                            string __genre = wordLower.TrimEnd(':').Trim().TrimEnd(')').TrimStart('(');
                            Attributes.Add("genre", __genre);
                            isWaitingGenre = false;
                            isWaitingAttributes = true;
                        }
                        else if (isWaitingAttributes)
                        {
                            for (int p =0;p <word.Length;p++)
                            {
                                //For each char of word
                                char c = word[p];
                                char c2 = ' '; //second char

                                bool isError = false;
                                Error error = new Error();

                                if (p + 1 < word.Length) c2 = word[p + 1];
                                
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
                                            //LineLen++;
                                            //continue;
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
                                                    lineC = i,
                                                    lenC = LineLen + j
                                                };
                                                isError = true;
                                                error = err;
                                                //ErrorHandle.ErrMessageBuilder.Build(
                                                //    ErrorHandle.ErrorPathCodes.Parser,
                                                //    0,
                                                //    2, //DevCode
                                                //    "Spilled out obscure object",
                                                //    masterPagePath,
                                                //    j,
                                                //    LineLen + j,
                                                //    line
                                                //    ).Print();
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

                                            }
                                            else
                                            {
                                                if (c == '"')
                                                {
                                                    isAttributeBuildContentStart = true;
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
                                                        lineC = i,
                                                        lenC = LineLen + j
                                                    };
                                                    isError = true;
                                                    error = err;
                                                    //ErrorHandle.ErrMessageBuilder.Build(err).Print();
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
                                            lineC = i,
                                            lenC = LineLen + j
                                        };
                                        isError = true;
                                        error = err;
                                        //ErrorHandle.ErrMessageBuilder.Build(err).Print();
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

                        if (wordLower == "set")
                        {
                            isProgress = true;
                            ProgressSyntax = "set";
                            isWaitingName = true;
                        }                        
                    }

                    //For each word of line end
                }

                //For each line end
            }
        }
    }
}
