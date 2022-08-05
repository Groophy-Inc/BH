using System;
using System.Collections.Generic;
using BH.Structes;
using BH.Parser;
using BH.ErrorHandle;
using ANSIConsole;

namespace BH.Parser.Commands
{
    internal class Var
    {
        public static void Decompose()
        {
            if (Parse.Var_ContentStart && Parse.wordNumber != 0)
            {
                Parse.Var_Content += " ";
            }

            if (Parse.Var_isWaitingVarName)
            {
                if (Parse.wordLower.StartsWith("$"))
                {
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = Parse.wordLower.Substring(1);
                    Parse.Var_isWaitingConKey = true;
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 2,
                        DevCode = 0,
                        ErrorMessage = "Varriable not found, You may have forgotten to put '$'.",
                        FilePath = Parse.masterPagePath,
                        line = Parse.line,
                        lineC = Parse.LineC,
                        lenC = Parse.TotalIndexOfLineWords
                    };
                    ErrorHandle.ErrorStack.PrintStack(new Error[] { err });
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.Var_isWaitingConKey)
            {
                if (Parse.wordLower.Equals("->"))
                {
                    Parse.Var_isWaitingConKey = false;
                    Parse.Var_isWaitingASorContentStart = true;
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 3,
                        DevCode = 0,
                        ErrorMessage = "ConKey not found, You may have forgotten to put \"->\".",
                        FilePath = Parse.masterPagePath,
                        line = Parse.line,
                        lineC = Parse.LineC,
                        lenC = Parse.TotalIndexOfLineWords
                    };
                    ErrorHandle.ErrorStack.PrintStack(new Error[] { err });
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.Var_isWaitingASorContentStart)
            {
                if (Parse.wordLower == "as")
                {
                    Parse.Var_isWaitingASorContentStart = false;
                    Parse.Var_isAS = true;
                }
                else if (Parse.wordLower.StartsWith("\""))
                {
                    Parse.Var_isWaitingASorContentStart = false;
                    Parse.Var_ContentStart = true;
                    ContentSearch(Parse.word.Substring(1), Parse.wordLower.Substring(1));

                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 4,
                        DevCode = 0,
                        ErrorMessage = "Content start key('\"') not found.",
                        FilePath = Parse.masterPagePath,
                        line = Parse.line,
                        lineC = Parse.LineC,
                        lenC = Parse.TotalIndexOfLineWords
                    };
                    ErrorHandle.ErrorStack.PrintStack(new Error[] { err });
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.Var_isAS)
            {
                Parse.Var_ASLang = Parse.wordLower;
                Parse.Var_isAS = false;

                if (Parse.Var_ASLang != "cf" || Parse.Var_ASLang != "ps" || Parse.Var_ASLang != "s+" || Parse.Var_ASLang != "c#")
                {
                    //error
                }
            }
            else if (Parse.Var_ContentStart)
            {
                ContentSearch(Parse.word, Parse.wordLower);
            }
            else if (Parse.Var_isWaitingSemiColon)
            {
                if (Parse.word == ";")
                {
                    Parse.isProgress = false;
                    Parse.ProgressSyntax = "";
                    Parse.isBackslashableContent = false;
                    Varriables.AddorUpdate(Parse.Var_VarName, Parse.Var_Content, "S:"+Parse.Var_ASLang);
                    LogSystem.log("New varriable as $" + Parse.Var_VarName + "\r\nCont -> '" + Parse.Var_Content + "'\r\nLang -> '" + Parse.Var_ASLang + "'");
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.Var_isWaitingConKey = false;
                    Parse.Var_isWaitingASorContentStart = false;
                    Parse.Var_isAS = false;
                    Parse.Var_ASLang = "";
                    Parse.Var_ContentStart = false;
                    Parse.Var_Content = "";
                    Parse.Var_isAttributeBackSlash = false;
                    Parse.Var_isWaitingSemiColon = false;
                }
                else if (Parse.word.StartsWith(';'))
                {
                    //error
                }
                else
                {
                    //error
                }
            }
        }

        private static void ContentSearch(string word,string wordLower)
        {
            for (int p = 0; p < word.Length; p++)
            {
                //For each char of word
                char _c = ' ';
                char c = word[p];
                char c_ = ' '; //second char

                bool isError = false;
                Error error = new Error();

                if (p + 1 < word.Length) c_ = word[p + 1];
                if (p - 1 < word.Length && p - 1 >= 0) _c = word[p - 1];

                if (Parse.Var_ContentStart)
                {
                    if (c == '\\')
                    {
                        Parse.Var_isAttributeBackSlash = true;
                    }
                    else
                    {
                        if (Parse.Var_isAttributeBackSlash)
                        {
                            Parse.Var_Content += c;
                            Parse.Var_isAttributeBackSlash = false;
                        }
                        else
                        {
                            if (c == '"')
                            {
                                Parse.Var_ContentStart = false;
                                Parse.Var_isWaitingSemiColon = true;
                            }
                            else
                            {
                                Parse.Var_Content += c;
                            }
                        }
                    }
                }
                else if (Parse.Var_isWaitingSemiColon)
                {
                    if (c == ';')
                    {
                        //end
                        Parse.isProgress = false;
                        Parse.ProgressSyntax = "";
                        Parse.isBackslashableContent = false;
                        Varriables.AddorUpdate(Parse.Var_VarName, Parse.Var_Content, "S:" + Parse.Var_ASLang);
                        LogSystem.log("New varriable as $" + Parse.Var_VarName + "\r\nCont -> '" + Parse.Var_Content + "'\r\nLang -> '" + Parse.Var_ASLang + "'");
                        Parse.Var_isWaitingVarName = false;
                        Parse.Var_VarName = "";
                        Parse.Var_isWaitingConKey = false;
                        Parse.Var_isWaitingASorContentStart = false;
                        Parse.Var_isAS = false;
                        Parse.Var_ASLang = "";
                        Parse.Var_ContentStart = false;
                        Parse.Var_Content = "";
                        Parse.Var_isAttributeBackSlash = false;
                        Parse.Var_isWaitingSemiColon = false;
                    }
                }

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
    }
}
