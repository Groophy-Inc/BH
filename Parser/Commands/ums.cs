using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ANSIConsole;
using BH.ErrorHandle;
using BH.ErrorHandle.Error;
using BH.Script.Types;
using BH.Structes.ErrorStack;

namespace BH.Parser.Commands
{
    public enum IScriptTypes
    {
        Null, // null for waiting
        CF, //Terminal input     a.k.a CmdFunc
        PS, //PowerShell         a.k.a PowerShell
        SP, //Custom lang for BH a.k.a Script-Plus
        CS, //C# runtime parse   a.k.a C-Sharp[#]
    }

    public class IScript
    {
        public IScriptTypes Type { get; set; }

        /// <summary>
        /// is save output to a varriable 
        /// 
        /// Work for 
        ///   - CF
        ///   - PS
        ///   - SP
        /// </summary>
        public bool isSaveable { get; set; }

        public string SaveName { get; set; }
        public string Code { get; set; }
    }

    internal class ums
    {
        
        private static string[] types = new string[] { "CF", "PS", "SP", "CS" };
        public static string GetClosest(string text) => APF.Find_Probabilities.GetClosest(text, types);
        public static bool isHaveLang(string text) => types.Any(x => x == text);

        public static void Clear()
        {
            Parse.UMS_isWaitingHaveVarriableSaveOrNot = false;
            Parse.UMS_isSaveAsVarriable = false;
            Parse.UMS_SaveAsVarriableName = "";
            Parse.UMS_isWaitingAS = false;
            Parse.UMS_isWaitingLang = false;
            Parse.UMS_Lang = IScriptTypes.Null;
            Parse.UMS_isWaitingConKey = false;
            Parse.UMS_isWaitingContentKey = false;
            Parse.UMS_ContentStart = false;
            Parse.UMS_isAttributeBackSlash = false;
            Parse.UMS_isWaitingSemiColon = false;
            Parse.UMS_Content = "";
        }

        public static void Decompose()
        {
            if (Parse.UMS_ContentStart)
            {
                Parse.UMS_Content += " ";
            }
            
            if (Parse.UMS_isWaitingHaveVarriableSaveOrNot)
            {
                if (Parse.word.StartsWith('$'))
                {
                    Parse.UMS_isSaveAsVarriable = true;
                    Parse.UMS_SaveAsVarriableName = Parse.word.Substring(1);
                    Parse.UMS_isWaitingHaveVarriableSaveOrNot = false;
                    Parse.UMS_isWaitingAS = true;
                    Logs.Log("Save as " + Parse.UMS_SaveAsVarriableName);
                }
                else
                {
                    Parse.UMS_isSaveAsVarriable = false;
                    Parse.UMS_SaveAsVarriableName = "";
                    Parse.UMS_isWaitingAS = true;
                    Parse.UMS_isWaitingHaveVarriableSaveOrNot = false;
                    if (Parse.wordLower == "as")
                    {
                        Parse.UMS_isWaitingAS = false;
                        Parse.UMS_isWaitingLang = true;
                        Logs.Log("Without save");
                        return;
                    }
                    else
                    {
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 3,
                            DevCode = 0,
                            ErrorMessage = "ConKey not found, You may have forgotten to put \"as\".",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err);
                        Parse.EndProcess();
                        Clear();
                        Parse.isSkipThisLine = true;
                    }
                }
            }
            else if (Parse.UMS_isWaitingAS)
            {
                if (Parse.wordLower == "as")
                {
                    Parse.UMS_isWaitingAS = false;
                    Parse.UMS_isWaitingLang = true;
                    Logs.Log("Without save - found as");
                }
                else
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 3,
                        DevCode = 0,
                        ErrorMessage = "ConKey not found, You may have forgotten to put \"as\".",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Clear();
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.UMS_isWaitingLang)
            {
                if (Parse.word.Length >= 2 && isHaveLang(Parse.word.Substring(0, 2)))
                {
                    switch (Parse.word.Substring(0,2).ToUpper())
                    {
                        case "CF":
                            Parse.UMS_Lang = IScriptTypes.CF;
                            break;
                        case "PS":
                            Parse.UMS_Lang = IScriptTypes.PS;
                            break;
                        case "SP":
                            Parse.UMS_Lang = IScriptTypes.SP;
                            break;
                        case "CS":
                            Parse.UMS_Lang = IScriptTypes.CS;
                            break;
                    }
                    
                    Logs.Log("Lang: " + Parse.UMS_Lang.ToString());

                    Parse.UMS_isWaitingLang = false;
                    Parse.UMS_isWaitingConKey = true; //':'
                    Logs.Log("WordLen: " + Parse.word.Length);
                    if (Parse.word.Length >= 3)
                    {
                        string after = Parse.word.Substring(2);
                        Logs.Log("After: " + after);
                        int OthersLen = 2;
                        for (int i = 0; i < after.Length; i++)
                        {
                            if (Parse.UMS_isWaitingConKey)
                            {
                                if (after[i] == ':')
                                {
                                    Parse.UMS_isWaitingConKey = false;
                                    Parse.UMS_isWaitingContentKey = true;
                                    Logs.Log("Conkey(:) found.");
                                    OthersLen++;
                                }
                                else
                                {
                                    Error err = new Error()
                                    {
                                        ErrorPathCode = ErrorPathCodes.Parser,
                                        ErrorID = 3,
                                        DevCode = 0,
                                        ErrorMessage = "ConKey not found, You may have forgotten to put \":\".",
                                        HighLightLen = Parse.word.Length,
                                        line = Parse.line,
                                    };
                                    ErrorStack.PrintStack(err);
                                    Parse.EndProcess();
                                    Clear();
                                    Parse.isSkipThisLine = true;
                                }
                            }
                            else if (Parse.UMS_isWaitingContentKey)
                            {
                                if (after[i] == '"')
                                {
                                    OthersLen++;
                                    Parse.UMS_isWaitingContentKey = false;
                                    Parse.UMS_ContentStart = true;
                                    Logs.Log("ContentKey(\") found.");
                                    string UnifiedContent = after.Substring(2);
                                    ContentSearch(Parse.word.Substring(OthersLen), Parse.wordLower.Substring(OthersLen));
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
                                    Clear();
                                    Parse.isSkipThisLine = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string getClosest = GetClosest(Parse.word);
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 5,
                        DevCode = 0,
                        ErrorMessage = "Lang not found, this might be what you're looking for: '" +
                                       getClosest.Color(ConsoleColor.Green) + "'.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err);
                    Parse.EndProcess();
                    Clear();
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.UMS_isWaitingConKey)
            {
                int OthersLen = 0;
                for (int i = 0; i < Parse.word.Length; i++)
                {
                    if (Parse.UMS_isWaitingConKey)
                    {
                        if (Parse.word[i] == ':')
                        {
                            Parse.UMS_isWaitingConKey = false;
                            Parse.UMS_isWaitingContentKey = true;
                            OthersLen++;
                            Logs.Log("ContentKey(:) found.");
                        }
                        else
                        {
                            Error err = new Error()
                            {
                                ErrorPathCode = ErrorPathCodes.Parser,
                                ErrorID = 3,
                                DevCode = 0,
                                ErrorMessage = "ConKey not found, You may have forgotten to put \":\".",
                                HighLightLen = Parse.word.Length,
                                line = Parse.line,
                            };
                            ErrorStack.PrintStack(err);
                            Parse.EndProcess();
                            Clear();
                            Parse.isSkipThisLine = true;
                        }
                    }
                    else if (Parse.UMS_isWaitingContentKey)
                    {
                        if (Parse.word[i] == '"')
                        {
                            OthersLen++;
                            Parse.UMS_isWaitingContentKey = false;
                            Parse.UMS_ContentStart = true;
                            string UnifiedContent = Parse.word.Substring(2);
                            Logs.Log("ContentKey(\") found.");
                            ContentSearch(Parse.word.Substring(OthersLen), Parse.wordLower.Substring(OthersLen));
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
                            Clear();
                            Parse.isSkipThisLine = true;
                        }
                    }
                }
            }
            else if (Parse.UMS_isWaitingContentKey)
            {
                if (Parse.word.StartsWith('"'))
                {
                    Parse.UMS_isWaitingContentKey = false;
                    Parse.UMS_ContentStart = true;
                    Logs.Log("ContentKey(\") found.");
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
                    Clear();
                    Parse.isSkipThisLine = true;
                }
            }
            else if (Parse.UMS_ContentStart)
            {
                ContentSearch(Parse.word, Parse.wordLower);
            }
            else if (Parse.UMS_isWaitingSemiColon)
            {
                ContentSearch(Parse.word, Parse.wordLower);
            }
        }
        
        private static void ContentSearch(string word,string wordLower)
        {
            Logs.Log("Content Search: '"+word+"'");
            Logs.LogW("      | ");
            for (int p = 0; p < word.Length; p++)
            {
                //For each char of word
                char _c = ' ';
                char c = word[p];
                char c_ = ' '; //second char

                if (p + 1 < word.Length) c_ = word[p + 1];
                if (p - 1 < word.Length && p - 1 >= 0) _c = word[p - 1];
              

                if (Parse.UMS_ContentStart)
                {
                    if (c == '\\')
                    {
                        Parse.UMS_isAttributeBackSlash = true;
                    }
                    else
                    {
                        if (Parse.UMS_isAttributeBackSlash)
                        {
                            Parse.UMS_Content += c;
                            Logs.LogW(c);
                            Parse.UMS_isAttributeBackSlash = false;
                        }
                        else
                        {
                            if (c == '"')
                            {
                                Parse.UMS_ContentStart = false;
                                Parse.isAnyContent = false;
                                Logs.Log("\r\n");
                                Logs.Log("isAnyContent FALSE");
                                Parse.UMS_isWaitingSemiColon = true;
                                Logs.Log("DEVLOG - Var_isWaitingSemiColon TRUE");
                                Logs.Log("UMS_Content: '" + Parse.UMS_Content+"'");
                            }
                            else
                            {
                                Parse.UMS_Content += c; 
                                Logs.LogW(c);
                            }
                        }
                    }
                }
                else if (Parse.UMS_isWaitingSemiColon)
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
                            Clear();
                            Parse.isSkipThisLine = true;
                            continue;
                        }
                        //end
                        Parse.EndProcess();
                        Parse.UMS_Content = Varriables.FixContent(Parse.UMS_Content);
                        switch (Parse.UMS_Lang)
                        {
                            case IScriptTypes.CF:
                                var CFOBJ = Script.Types.CF.Execute(Parse.UMS_Content, 3000);
                                Varriables.AddorUpdate(Parse.UMS_SaveAsVarriableName, CFOBJ, "S:" + Parse.UMS_Lang);
                                break;
                            case IScriptTypes.PS:
                                var PSOBJ = Script.Types.PS.Execute(Parse.UMS_Content, 3000);
                                Varriables.AddorUpdate(Parse.UMS_SaveAsVarriableName, PSOBJ, "S:" + Parse.UMS_Lang);
                                break;
                            case IScriptTypes.CS:
                                var CSOBJ = Script.Types.CS.Execute(Parse.UMS_Content, 3000);
                                Varriables.AddorUpdate(Parse.UMS_SaveAsVarriableName, CSOBJ, "S:" + Parse.UMS_Lang);
                                break;
                        }
                        Logs.Log("New varriable as $" + Parse.UMS_SaveAsVarriableName + "\r\nCont -> '" + Parse.UMS_Content + "'\r\nLang -> '" + Parse.UMS_Lang + "'");
                        Logs.Log("Process end;");
                        Parse.isAnyContent = false;
                    }
                }

                Parse.LineLen++;
            }
        }
    }
}