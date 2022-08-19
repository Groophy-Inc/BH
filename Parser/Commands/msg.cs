using System;
using System.Collections.Generic;
using System.Linq;
using BH.Structes;
using BH.Parser;
using BH.ErrorHandle;
using ANSIConsole;

namespace BH.Parser.Commands
{
    internal class Msg
    {
        public static void Decompose()
        {
            if (Parse.Msg_isContentStart && Parse.wordNumber != 1)
            {
                Parse.Msg_Content += " ";
            }

            int lastestIndex = 0;
            bool skipCheck = false;
            if (Parse.Msg_isWaitingContentStart)
            {

                for (int p = 0; p < Parse.word.Length; p++)
                {
                    //For each char of word
                    char c = Parse.word[p];

                    if (c == '"')
                    {
                        Parse.Msg_isContentStart = true;
                        Parse.isAnyContent = true;
                        Parse.Msg_isWaitingContentStart = false;
                        Parse.isBackslashableContent = true;
                        lastestIndex = p+1;
                        break;
                    }
                    else
                    {

                        /*Error err = new Error()
                        {
                            ErrorPathCode = ErrorPathCodes.Parser,
                            ErrorID = 0,
                            DevCode = 3,
                            ErrorMessage = "Spilled out obscure object",
                            FilePath = Parse.masterPagePath,
                            line = Parse.line,
                            lineC = Parse.LineC,
                            lenC = Parse.TotalIndexOfLineWords + p
                        };
                        isError = true;
                        error = err;*/
                        Parse.Msg_ContentWithoutConKey = true;
                        Parse.isAnyContent = true;
                        Parse.Msg_isWaitingContentStart = false;
                        Parse.isBackslashableContent = true;
                        lastestIndex = p + 1;
                    }




                    Parse.LineLen++;
                }
            }
            
            if (skipCheck)
            {
                skipCheck = false;

            }
            else
            {
                if (Parse.Msg_isContentStart)
                {
                    for (int p = lastestIndex; p < Parse.word.Length; p++)
                    {
                        //For each char of word
                        char _c = ' ';
                        char c = Parse.word[p];
                        char c_ = ' '; //second char

                        if (p + 1 < Parse.word.Length) c_ = Parse.word[p + 1];
                        if (p - 1 < Parse.word.Length && p - 1 >= 0) _c = Parse.word[p - 1];

                        if (!Parse.isProgress)
                        {
                            Logs.Log("The parameter persists even after the content has expired, this is not an error cause but an unnecessary use. Ln: "+Parse.LineC+", ChLn: " + (Parse.TotalIndexOfLineWords + p).ToString()+"\r\nLine: " + Parse.line);
                        }

                        if (c == '\\' && c_ != '$')
                        {
                            Parse.Msg_isBackSlash = true;
                            Parse.LineLen++;
                            continue;
                        }

                        if (c == '"')
                        {
                            if (Parse.Msg_isBackSlash)
                            {
                                Parse.Msg_Content += c;
                                Parse.Msg_isBackSlash = false;
                            }
                            else
                            {


                                Logs.Log("msg end: '" + Parse.Msg_Content + "'");

                                ANSIIConsole.Gecho.Print(Varriables.FixContent(Parse.Msg_Content));

                                Parse.Msg_Content = "";
                                Parse.Msg_isBackSlash = false;
                                Parse.Msg_isContentStart = false;
                                Parse.isAnyContent = false;
                                Parse.Msg_isWaitingContentStart = false;
                                Parse.EndProcess();
                                //content end
                            }
                        }
                        else
                        {
                            if (Parse.Msg_isBackSlash)
                            {
                                Parse.Msg_Content += c;
                                Parse.Msg_isBackSlash = false;
                            }
                            else
                            {
                                Parse.Msg_Content += c;
                            }
                        }

                        Parse.LineLen++;
                    }
                }
                else if (Parse.Msg_ContentWithoutConKey)
                {
                    Logs.Log("msg end: '" + Parse.line.Substring(Parse.TotalIndexOfLineWords) + "'");

                    ANSIIConsole.Gecho.Print(Varriables.FixContent(Parse.line.Substring(Parse.TotalIndexOfLineWords)));

                    Parse.Msg_Content = "";
                    Parse.Msg_isBackSlash = false;
                    Parse.Msg_isContentStart = false;
                    Parse.isAnyContent = false;
                    Parse.Msg_isWaitingContentStart = false;
                    Parse.isSkipThisLine = true;
                    Parse.EndProcess();
                }
            }
        }
    }
}
