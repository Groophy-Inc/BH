using System;
using System.Collections.Generic;
using BH.Structes;
using BH.Parser;
using BH.ErrorHandle;
using ANSIConsole;

namespace BH.Parser.Commands
{
    internal class Set
    {
        public static Tuple<bool, Element> Decompose()
        {
            if (Parse.Set_isAttributeBuildContent && Parse.wordNumber != 0)
            {
                Parse.Set_AttributeBuildContent += " ";
            }

            if (Parse.Set_isWaitingName)
            {
                string __name = Parse.word;
                if (Parse.word[0] == '$') __name = __name.Substring(1);
                Parse.Set_Attributes.Add("name", __name);
                Parse.Set_isWaitingName = false;
                Parse.Set_isWaitingGenre = true;
                LogSystem.log("New Attribute - '" + "name" + "': " + __name, ConsoleColor.Green);
            }
            else if (Parse.Set_isWaitingGenre)
            {
                string __genre = Parse.wordLower.TrimEnd(':').Trim().TrimEnd(')').TrimStart('(');
                Parse.Set_Attributes.Add("genre", __genre);
                Parse.Set_isWaitingGenre = false;
                Parse.Set_isWaitingAttributes = true;
                LogSystem.log("New Attribute - '" + "genre" + "': " + __genre, ConsoleColor.Green);
            }
            else if (Parse.Set_isWaitingAttributes)
            {
                for (int p = 0; p < Parse.word.Length; p++)
                {
                    //For each char of word
                    char _c = ' ';
                    char c = Parse.word[p];
                    char c_ = ' '; //second char

                    bool isError = false;
                    Error error = new Error();




                    if (p + 1 < Parse.word.Length) c_ = Parse.word[p + 1];
                    if (p - 1 < Parse.word.Length && p - 1 >= 0) _c = Parse.word[p - 1];

                    if (Parse.Set_isAttributeBuild)
                    {
                        if (Parse.Set_isAttributeBuildName)
                        {
                            //key
                            if (c == '"')
                            {
                                Parse.Set_isAttributeBuildName = false;

                            }
                            else
                            {
                                Parse.Set_AttributeBuildName += c;
                            }
                        }
                        else
                        {
                            if (c == ':')
                            {
                                Parse.Set_isAttributeBuildContent = true;
                                Parse.isSkipSecondCheck = true;
                            }
                            else
                            {
                                if (Parse.Set_isAttributeBuildName)
                                {

                                    Error err = new Error()
                                    {
                                        ErrorPathCode = ErrorPathCodes.Parser,
                                        ErrorID = 0,
                                        DevCode = 2,
                                        ErrorMessage = "Spilled out obscure object",
                                        FilePath = Parse.masterPagePath,
                                        line = Parse.line,
                                        lineC = Parse.LineC,
                                        lenC = Parse.LenC
                                    };
                                    isError = true;
                                    error = err;
                                }
                            }

                            //value
                            if (Parse.isSkipSecondCheck)
                            {
                                Parse.isSkipSecondCheck = false;
                            }
                            else
                            {
                                if (Parse.Set_isAttributeBuildContentStart)
                                {
                                    if (c == '\\')
                                    {
                                        Parse.Set_isAttributeBackSlash = true;
                                    }
                                    else
                                    {
                                        if (Parse.Set_isAttributeBackSlash)
                                        {
                                            Parse.Set_AttributeBuildContent += c;
                                            Parse.Set_isAttributeBackSlash = false;
                                        }
                                        else
                                        {
                                            if (c == '"')
                                            {
                                                Parse.Set_isAttributeBuildContentStart = false;
                                                Parse.Set_isAttributeBuildContent = false;
                                                Parse.Set_isAttributeBuild = false;
                                                Parse.isBackslashableContent = false;
                                                Parse.Set_Attributes.Add(Parse.Set_AttributeBuildName, Parse.Set_AttributeBuildContent);
                                                LogSystem.log("New Attribute - '" + Parse.Set_AttributeBuildName + "': " + Parse.Set_AttributeBuildContent, ConsoleColor.Green);
                                                Parse.Set_AttributeBuildName = "";
                                                Parse.Set_AttributeBuildContent = "";
                                                Parse.Set_isNewAttributeWaiting = true;
                                            }
                                            else
                                            {
                                                Parse.Set_AttributeBuildContent += c;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (c == '"')
                                    {
                                        if (Parse.Set_isAttributeBuildContent)
                                        {
                                            Parse.Set_isAttributeBuildContentStart = true;
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
                                            FilePath = Parse.masterPagePath,
                                            line = Parse.line,
                                            lineC = Parse.LineC,
                                            lenC = Parse.LenC
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
                            Parse.Set_isAttributeBuild = true;
                            Parse.isBackslashableContent = true;
                            Parse.Set_isAttributeBuildName = true;
                        }
                        else if (c == ',')
                        {
                            Parse.Set_isAttributeBuild = false;
                            Parse.isBackslashableContent = false;
                        }
                        else if (c == ';')
                        {
                            var retenv = new Element()
                            {
                                Genre = Parse.Set_Attributes["genre"],
                                Attributes = Parse.Set_Attributes
                            };

                            LogSystem.log("New element added to element list name of $" + Parse.Set_Attributes["name"].Color(ConsoleColor.Magenta), ConsoleColor.DarkGreen);

                            Parse.isProgress = false;
                            Parse.ProgressSyntax = "";
                            Parse.isBackslashableContent = false;
                            Parse.Set_isWaitingName = false;
                            Parse.Set_isWaitingGenre = false;
                            Parse.Set_isWaitingAttributes = false;
                            Parse.Set_isAttributeBuild = false;
                            Parse.Set_isAttributeBuildName = false;
                            Parse.Set_AttributeBuildName = "";
                            Parse.Set_isAttributeBuildContentStart = false;
                            Parse.Set_isAttributeBackSlash = false;
                            Parse.Set_isAttributeBuildContent = false;
                            Parse.Set_AttributeBuildContent = "";
                            Parse.Set_isNewAttributeWaiting = false;
                            Parse.Set_Attributes = new System.Collections.Generic.Dictionary<string, string>();

                            return new Tuple<bool, Element>(true, retenv);
                        }
                        else
                        {
                            Error err = new Error()
                            {
                                ErrorPathCode = ErrorPathCodes.Parser,
                                ErrorID = 0,
                                DevCode = 1,
                                ErrorMessage = "Spilled out obscure object",
                                FilePath = Parse.masterPagePath,
                                line = Parse.line,
                                lineC = Parse.LineC,
                                lenC = Parse.LenC
                            };
                            isError = true;
                            error = err;
                        }
                    }

                    //Check error stack
                    if (Parse.isErrorStack)
                    {
                        if (isError)
                        {
                            if (error.ErrorID == Parse.StackErrorID)
                            {
                                Parse.errors.Add(error);
                            }
                            else //new error stack
                            {
                                ErrorStack.PrintStack(Parse.errors.ToArray());
                                Parse.isErrorStack = true;
                                Parse.errors = new System.Collections.Generic.List<Error>();
                                Parse.StackErrorID = error.ErrorID;
                            }
                        }
                        else
                        {
                            ErrorStack.PrintStack(Parse.errors.ToArray());
                            Parse.isErrorStack = false;
                            Parse.errors = new System.Collections.Generic.List<Error>();
                            Parse.StackErrorID = -1;
                        }
                    }
                    else
                    {
                        if (isError)
                        {
                            Parse.isErrorStack = true;
                            Parse.StackErrorID = error.ErrorID;
                            Parse.errors.Add(error);
                        }
                    }

                    Parse.LineLen++;
                }
            }
            return new Tuple<bool, Element>(false, null);
        }
    }
}
