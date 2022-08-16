using System;
using System.Collections.Generic;
using BH.Structes;
using BH.Parser;
using BH.ErrorHandle;
using ANSIConsole;
using BH.ErrorHandle.Error;

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
                Logs.Log("DEVLOG - Var_isWaitingVarName\r\n");
                if (Parse.wordLower.StartsWith("$"))
                {
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = Parse.wordLower.Substring(1);
                    Parse.Var_isWaitingConKey = true;
                    Logs.Log($"VarriableName: '{Parse.Var_VarName}'");
                    Logs.Log("DEVLOG - Var_isWaitingConKey");
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 2,
                        DevCode = 0,
                        ErrorMessage = "Varriable not found, You may have forgotten to put '$'.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
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
                    Logs.Log("ConKey found.");
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 3,
                        DevCode = 0,
                        ErrorMessage = "ConKey not found, You may have forgotten to put \"->\".",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.Var_isWaitingASorContentStart)
            {
                if (Parse.wordLower == "as")
                {

                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 10,
                        DevCode = 0,
                        ErrorMessage = "The as suffix was removed with version 0.1.1 of BH.".Color(ConsoleColor.Red).ToString(),
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Parse.isSkipThisLine = true;
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
                else if (Parse.wordLower.StartsWith("\""))
                {
                    Parse.Var_isWaitingASorContentStart = false;
                    Parse.Var_ContentStart = true;
                    Parse.isAnyContent = true;
                    Logs.Log("isAnyContent TRUE");
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
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
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
                    string getClosest = APF.Find_Probabilities.GetClosest(Parse.Var_ASLang, new string[]{ "cf", "ps", "s+", "c#"} );
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 5,
                        DevCode = 0,
                        ErrorMessage = "Lang not found, this might be what you're looking for: '" + getClosest.Color(ConsoleColor.Green) + "'.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.isSkipThisLine = true;
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
                    Parse.EndProcess();
                   
                    Varriables.AddorUpdate(Parse.Var_VarName, Parse.Var_Content, "S:"+Parse.Var_ASLang);
                    Logs.Log("New varriable as $" + Parse.Var_VarName + "\r\nCont -> '" + Parse.Var_Content + "'\r\nLang -> '" + Parse.Var_ASLang + "'");
                    Logs.Log("Process end;");
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.Var_isWaitingConKey = false;
                    Parse.Var_isWaitingASorContentStart = false;
                    Parse.Var_isAS = false;
                    Parse.Var_ASLang = "";
                    Parse.Var_ContentStart = false;
                    Parse.isAnyContent = false;
                    Parse.Var_Content = "";
                    Parse.Var_isAttributeBackSlash = false;
                    Parse.Var_isWaitingSemiColon = false;
                }
                else if (Parse.word.StartsWith(';'))
                {
                    //error
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 6,
                        DevCode = 0,
                        ErrorMessage = "End key not found. You should leave a space after ';' because of the word by word parse.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.isSkipThisLine = true;
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 6,
                        DevCode = 0,
                        ErrorMessage = "End key not found. #*#",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Parse.Var_isWaitingVarName = false;
                    Parse.Var_VarName = "";
                    Parse.isSkipThisLine = true;
                }
            }
        }

        private static void ContentSearch(string word,string wordLower)
        {
            Logs.LogW("      | ");
            for (int p = 0; p < word.Length; p++)
            {
                //For each char of word
                char _c = ' ';
                char c = word[p];
                char c_ = ' '; //second char

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
                            Logs.LogW(c);
                            Parse.Var_isAttributeBackSlash = false;
                        }
                        else
                        {
                            if (c == '"')
                            {
                                Parse.Var_ContentStart = false;
                                Parse.isAnyContent = false;
                                Logs.Log("\r\n");
                                Logs.Log("isAnyContent FALSE");
                                Parse.Var_isWaitingSemiColon = true;
                                Logs.Log("DEVLOG - Var_isWaitingSemiColon");
                            }
                            else
                            {
                                Parse.Var_Content += c; 
                                Logs.LogW(c);
                            }
                        }
                    }
                }
                else if (Parse.Var_isWaitingSemiColon)
                {
                    if (c == ';')
                    {
                        if (c_ != ' ')
                        {
                            Error err = new Error()
                            {
                                ErrorPathCode = ErrorPathCodes.Parser,
                                ErrorID = 6,
                                DevCode = 1,
                                ErrorMessage = "End key not found. You should leave a space after ';' because of the word by word parse.",
                                HighLightLen = Parse.word.Length,
                                line = Parse.line,
                            };
                            ErrorStack.PrintStack(err);
                            Parse.EndProcess();
                            Parse.Var_isWaitingVarName = false;
                            Parse.Var_VarName = "";
                            Parse.isSkipThisLine = true;
                            continue;
                        }
                        //end
                        Parse.EndProcess();
                        Varriables.AddorUpdate(Parse.Var_VarName, Parse.Var_Content, "S:" + Parse.Var_ASLang);
                        Logs.Log("New varriable as $" + Parse.Var_VarName + "\r\nCont -> '" + Parse.Var_Content + "'\r\nLang -> '" + Parse.Var_ASLang + "'");
                        Logs.Log("Process end;");
                        Parse.Var_isWaitingVarName = false;
                        Parse.Var_VarName = "";
                        Parse.Var_isWaitingConKey = false;
                        Parse.Var_isWaitingASorContentStart = false;
                        Parse.Var_isAS = false;
                        Parse.Var_ASLang = "";
                        Parse.Var_ContentStart = false;
                        Parse.isAnyContent = false;
                        Parse.Var_Content = "";
                        Parse.Var_isAttributeBackSlash = false;
                        Parse.Var_isWaitingSemiColon = false;
                    }
                }

                Parse.LineLen++;
            }
        }
    }
}
