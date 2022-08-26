using System;
using System.Linq;
using ANSIConsole;
using BH.ErrorHandle.Error;
using BH.Structes.ErrorStack;

namespace BH.Parser.Commands
{
    public class Runner
    {
        public static void Run(ref pbp_Command rules, ref int left, ref bool end)
        {
            string word = Parse.wordLower;
            
            if (rules.Commands.Count == left + 1)
            {
                if (word.EndsWith(';'))
                {
                    word = word.TrimEnd(';');
                    end = true;
                }
            }
            else if (rules.Commands.Count < left + 1)
            {
                if (word == ";")
                {
                    end = true;
                    return;
                }
                else
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
                    ErrorStack.PrintStack(err, "Parser/Commands/CommandBuilder/Runner | 42");
                    Parse.EndProcess();
                    return;
                }
            }

            var rule = rules.Commands[left];
            switch (rule.Type)
            {
                case CommandParseType.signed_value: // [a]
                    if (word == rule.Value.ToString())
                    {
                        rule.Result = true;
                        left++;
                    }
                    else
                    {
                        rule.Result = false;
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 12,
                            DevCode = 0,
                            ErrorMessage = "Signed Value not found, You may have forgotten to put \""+rule.Value+"\".",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, "Parser/Commands/CommandBuilder/Runner | 69");
                        Parse.EndProcess();
                    }
                    break;
                case CommandParseType.signed_values: // [a|b]
                    if (((string[])rule.Value).Any(x => x == word))
                    {
                        rule.Result = new Tuple<bool, string>(true, word);
                        left++;
                    }
                    else
                    {
                        rule.Result = new Tuple<bool, string>(false, word);
                        string getClosest = APF.Find_Probabilities.GetClosest(word, (string[])rule.Value);
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 13,
                            DevCode = 0,
                            ErrorMessage = "Signed Values not found, this might be what you're looking for: '" + getClosest.Color("#"+Config.Parser.Config.Read("Suggestion","Error")) + "'.".Color(ConsoleColor.Yellow),
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, "Parser/Commands/CommandBuilder/Runner | 92");
                        Parse.EndProcess();
                    }
                    break;
                case CommandParseType.unsigned_value: // [$hi]
                    if (word.StartsWith('$'))
                    {
                        rule.Result = word;
                        left++;
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
                        ErrorStack.PrintStack(err, "Parser/Commands/CommandBuilder/Runner | 113");
                        Parse.EndProcess();
                    }
                    break;
                case CommandParseType.content: // [""]
                    var status = rule.StatusValue_NotForUsers;
                    if (status == 0) rule.Result = "";
                    var append = (string)rule.Result;
                    ContentSearch(Parse.word, ref status, ref append);
                    rule.StatusValue_NotForUsers = status;
                    rule.Result = append;
                    if (status == 3)
                    {
                        left++;
                    }
                    break;
            }
            if (end)
            {
                left = 0;
            }
        }

        //0 -> waiting content start
        //1 -> waiting content end
        //2 -> end
        //3 -> end with ;
        public static void ContentSearch(string Word, ref int status, ref string text_to_append)
        {
            bool isBackslash = false;
            foreach (char c in Word)
            {
                if (status == 0)
                {
                    if (c == '"')
                    {
                        status++;
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
                        ErrorStack.PrintStack(err, "Parser/Commands/CommandBuilder/Runner | 159");
                        Parse.EndProcess();
                        break;
                    }
                }
                else if (status == 1)
                {
                    if (isBackslash)
                    {
                        text_to_append += c;
                    }
                    else
                    {
                        if (c == '\\')
                        {
                            isBackslash = true;
                        }
                        else
                        {
                            if (c == '"')
                            {
                                status++;
                            }
                            else text_to_append += c;
                        }
                    }
                }
                else if (status == 2)
                {
                    if (c == ';')
                    {
                        status++;
                    }
                    else
                    {
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 6,
                            DevCode = 0,
                            ErrorMessage = "End key not found. You should leave a space after ';' because of the word by word parse.",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, "Parser/Commands/CommandBuilder/Runner | 202");
                        Parse.EndProcess();
                        break;
                    }
                }
                else if (status == 3)
                {
                    Error err = new Error()
                    {
                        ErrorPathCode = ErrorPathCodes.Parser,
                        ErrorID = 6,
                        DevCode = 0,
                        ErrorMessage = "End key not found. You should leave a space after ';' because of the word by word parse.",
                        HighLightLen = Parse.word.Length,
                        line = Parse.line,
                    };
                    ErrorStack.PrintStack(err, "Parser/Commands/CommandBuilder/Runner | 217");
                    Parse.EndProcess();
                    break;
                }
            }
        }
    }
}