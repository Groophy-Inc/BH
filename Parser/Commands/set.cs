using System;
using System.Collections.Generic;
using BH.Structes;
using BH.Parser;
using BH.ErrorHandle;
using ANSIConsole;
using BH.ErrorHandle.Error;
using BH.Structes.ErrorStack;

namespace BH.Parser.Commands
{
    internal class Set
    {
     
        public static void AddorUpdate(string key, string value)
        {
            if (!Parse.Set_Attributes.TryAdd(key, value)) Parse.Set_Attributes[key] = value;
        }

        public static Tuple<bool, Element> Decompose()
        {

            if (Parse.Set_isAttributeBuildContent && !string.IsNullOrEmpty(Parse.Set_AttributeBuildContent))
            {
                Parse.Set_AttributeBuildContent += " ";
            }

            if (Parse.Set_isWaitingName)
            {
                string __name = Parse.word;
                if (Parse.word[0] == '$') __name = __name.Substring(1);
                AddorUpdate("name", __name);
                Parse.Set_isWaitingName = false;
                Parse.Set_isWaitingGenre = true;
                Logs.Log("New Attribute - '" + "name" + "': " + __name, ConsoleColor.Green);
                Parse.ProjectName = __name;
            }
            else if (Parse.Set_isWaitingGenre)
            {
                string __genre = Parse.wordLower.TrimEnd(':').Trim().TrimEnd(')').TrimStart('(');
                AddorUpdate("genre", __genre);
                Parse.Set_isWaitingGenre = false;
                Parse.Set_isWaitingAttributes = true;
                Logs.Log("New Attribute - '" + "genre" + "': " + __genre, ConsoleColor.Green);

                if (__genre == "window") AddorUpdate("Title", "NoThing"); //Defualt title 26.3.2022 || Sad but life is never said will be easy
            }
            else if (Parse.Set_isWaitingAttributes)
            {
                for (int p = 0; p < Parse.word.Length; p++)
                {
                    //For each char of word
                    char _c = ' ';
                    char c = Parse.word[p];
                    char c_ = ' '; //second char

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
                                Parse.isAnyContent = false;

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
                                        HighLightLen = Parse.word.Length,
                                        line = Parse.line,
                                    };
                                    ErrorStack.PrintStack(err, "Parser/Commands/set.cs | 97");
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
                                                Parse.isAnyContent = false;
                                                Parse.isBackslashableContent = false;
                                                AddorUpdate(Parse.Set_AttributeBuildName, Varriables.FixContent(Parse.Set_AttributeBuildContent));
                                                Logs.Log("New Attribute - '" + Parse.Set_AttributeBuildName + "': " + Varriables.FixContent(Parse.Set_AttributeBuildContent), ConsoleColor.Green);
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
                                            Parse.isAnyContent = true;
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
                                            HighLightLen = Parse.word.Length,
                                            line = Parse.line,
                                        };
                                        ErrorStack.PrintStack(err, "Parser/Commands/set.cs | 164");
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
                            Parse.isAnyContent = true;
                            Parse.Set_isAttributeBuildName = true;
                        }
                        else if (c == ',')
                        {
                            Parse.Set_isAttributeBuild = false;
                            Parse.isAnyContent = false;
                            Parse.isBackslashableContent = false;
                        }
                        else if (c == ';')
                        {
                            var retenv = new Element()
                            {
                                Genre = Parse.Set_Attributes["genre"],
                                Attributes = Parse.Set_Attributes
                            };

                            Logs.Log("New element added to element list name of $" + Parse.Set_Attributes["name"].Color(ConsoleColor.Magenta), ConsoleColor.DarkGreen);

                            Parse.EndProcess();
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
                            Parse.isAnyContent = false;
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
                                HighLightLen = Parse.word.Length,
                                line = Parse.line,
                            };
                            ErrorStack.PrintStack(err, "Parser/Commands/set.cs | 224");
                        }
                    }

                    Parse.LineLen++;
                }
            }
            return new Tuple<bool, Element>(false, null);
        }
    }
}
