using System;
using System.Collections.Generic;
using System.Linq;
using ANSIConsole;
using BH.ErrorHandle.Error;
using BH.Structes.ErrorStack;

namespace BH.Parser.Commands
{
    public class Runner
    {
        public static bool isContent = false;
        
        public static void Run(ref pbp_Command rules, ref int left, ref bool end)
        {
            string word = Parse.wordLower;
            string NoLower = Parse.word;

            while (!string.IsNullOrEmpty(word))
            {
                if (rules.Commands.Count == left + 1)
                {
                    if (word.EndsWith(';') && !isContent)
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
                            ErrorMessage =
                                "End key not found. You should leave a space after ';' because of the word by word parse.",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                        Parse.EndProcess();
                        return;
                    }
                }

                var rule = rules.Commands[left];
                switch (rule.Type)
                {
                    case CommandParseType.signed_value: // [a]
                        if (word.StartsWith(rule.Value.ToString()))
                        {
                            rule.Result = true;
                            word = word.Substring(rule.Value.ToString().Length);
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
                                ErrorMessage = "Signed Value not found, You may have forgotten to put \"" + rule.Value +
                                               "\".",
                                HighLightLen = Parse.word.Length,
                                line = Parse.line,
                            };
                            ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                            Parse.EndProcess();
                        }

                        break;
                    case CommandParseType.signed_values: // [a|b]
                        if (((string[])rule.Value).Any(x => NoLower.StartsWith(x)))
                        {
                            var result = ((string[])rule.Value).Where(x => NoLower.StartsWith(x)).First();
                            rule.Result = new Tuple<bool, string>(true, result);
                            word = word.Substring(result.Length);
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
                                ErrorMessage = "Signed Values not found, this might be what you're looking for: '" +
                                               getClosest.Color(Config.Parser.Config.Read("Suggestion", "Error")) +
                                               "'.".Color(ConsoleColor.Yellow),
                                HighLightLen = Parse.word.Length,
                                line = Parse.line,
                            };
                            ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                            Parse.EndProcess();
                        }

                        break;
                    case CommandParseType.unsigned_value: // [$hi]
                        if (word.StartsWith('$'))
                        {
                            rule.Result = NoLower;
                            word = "";
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
                            ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                            Parse.EndProcess();
                        }

                        break;
                    case CommandParseType.content: // [""]
                        var status = rule.StatusValue_NotForUsers;
                        if (status == 0) rule.Result = "";

                        if (status != 0) //Check space or newline
                        {
                            if (Parse.isNewLine) rule.Result += "\r\n";
                            else rule.Result += " ";
                        }

                        var append = (string)rule.Result;
                        Tuple<bool, string> endleft = new Tuple<bool, string>(false, "");
                        ContentSearch(Parse.word, ref status, ref append, ref endleft);
                        rule.StatusValue_NotForUsers = status;
                        rule.Result = append;
                        if (status == 3)
                        {
                            rule.Result = Varriables.FixContent(((string)rule.Result));
                            left++;
                        }

                        if (endleft.Item1) word = endleft.Item2;
                        else word = "";

                        break;
                    case CommandParseType.attribute:
                        var statusA = rule.StatusValue_NotForUsers;
                        if (statusA == -1) {rule.Result = new attributes();
                            rule.StatusValue_NotForUsers++;
                            statusA++;
                        }

                        if (Parse.isNewLine) ((attributes)rule.Result).lastest_attr.appentToContent("\r\n", statusA); 
                        else ((attributes)rule.Result).lastest_attr.appentToContent(" ", statusA);

                        var appendA = (attributes)rule.Result;
                        Tuple<bool, string> endleftA = new Tuple<bool, string>(false, "");
                        AttrSearch(Parse.word, ref statusA, ref appendA, ref endleftA );
                        rule.StatusValue_NotForUsers = statusA;
                        rule.Result = appendA;
                        if (statusA == 6)
                        {
                            foreach (var attr in ((attributes)rule.Result).attr)
                            {
                                attr.value = Varriables.FixContent(attr.value);
                            }
                            left++;
                        }

                        if (endleftA.Item1) word = endleftA.Item2;
                        else word = "";
                        
                        break;
                }

                if (end)
                {
                    left = 0;
                }
            }
        }
        
        //-1 -> nothing to see
        //0 -> waiting contentkey start
        //1 -> waiting contentkey end
        //2 -> waiting ':'
        //3 -> waiting contentvalue start
        //4 -> waiting contentvalue end
        //5 -> if have ',' status -> 0
        //     else
        // 6   -\-> end  
        public static void AttrSearch(string Word, ref int status, ref attributes attr, ref Tuple<bool, string> isEnd_and_Extra)
        {
            bool isBackslash = false;
            foreach (char c in Word)
            {
                if (status == 0)
                {
                    if (c == '"')
                    {
                        status++;
                        isContent = true;
                    }
                    else
                    {
                        if (c == ' ') continue;
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 4,
                            DevCode = 0,
                            ErrorMessage = "Content start key('\"') not found.",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                        Parse.EndProcess();
                        break;
                    }
                }
                else if (status == 1)
                {
                    if (isBackslash)
                    {
                        attr.lastest_attr.key += c;
                        isBackslash = false;
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
                                isContent = false;
                            }
                            else attr.lastest_attr.key += c;
                        }
                    }
                }
                else if (status == 2)
                {
                    if (c == ':')
                    {
                        status++;
                    }
                    else
                    {
                        if (c == ' ') continue;
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 12,
                            DevCode = 0,
                            ErrorMessage = "Signed Value not found, You may have forgotten to put ':'",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                        Parse.EndProcess();
                        break;
                    }
                }
                else if (status == 3)
                {
                    if (c == '"')
                    {
                        status++;
                        isContent = true;
                    }
                    else
                    {
                        if (c == ' ') continue;
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 4,
                            DevCode = 0,
                            ErrorMessage = "Content start key('\"') not found.",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                        Parse.EndProcess();
                        break;
                    }
                }
                else if (status == 4)
                {
                    if (isBackslash)
                    {
                        attr.lastest_attr.value += c;
                        isBackslash = false;
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
                                isContent = false;
                            }
                            else attr.lastest_attr.value += c;
                        }
                    }
                }
                else if (status == 5)
                {
                    if (c == ',')
                    {
                        status = 0;
                        attr.attr.Add(attr.lastest_attr);
                        attr.lastest_attr = new attribute();
                    }
                    else if (c == '.')
                    {
                        status++;
                        attr.attr.Add(attr.lastest_attr);
                    }
                    else
                    {
                        if (c == ' ') continue;
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 12,
                            DevCode = 0,
                            ErrorMessage = "Signed Value not found, You may have forgotten to put ',' or '.'",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                        Parse.EndProcess();
                        break;
                    }
                }
                else if (status == 6)
                {
                    isEnd_and_Extra = new Tuple<bool, string>(true, isEnd_and_Extra.Item2 + c);
                }
            }
        }

        //0 -> waiting content start
        //1 -> waiting content end
        //2 -> end
        //3 -> end with ;
        public static void ContentSearch(string Word, ref int status, ref string text_to_append, ref Tuple<bool, string> isEnd_and_Extra)
        {
            bool isBackslash = false;
            foreach (char c in Word)
            {
                if (status == 0)
                {
                    if (c == '"')
                    {
                        status++;
                        isContent = true;
                    }
                    else
                    {
                        if (c == ' ') continue;
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 4,
                            DevCode = 0,
                            ErrorMessage = "Content start key('\"') not found.",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
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
                                isContent = false;
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
                        if (c == ' ') continue;
                        Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 6,
                            DevCode = 0,
                            ErrorMessage =
                                "End key not found. You should leave a space after ';' because of the word by word parse.",
                            HighLightLen = Parse.word.Length,
                            line = Parse.line,
                        };
                        ErrorStack.PrintStack(err, new System.Diagnostics.StackFrame(0, true));
                        Parse.EndProcess();
                        break;
                    }
                }
                else if (status == 3)
                {
                    isEnd_and_Extra = new Tuple<bool, string>(true, isEnd_and_Extra.Item2 + c);
                }
            }
        }
    }
}