using BH.Parser.Utils;
using System.Text;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using ANSIConsole;
using BH.Structes.ErrorStack;
using BH.Parser.Config;

namespace BH.ErrorHandle.Error
{
    
    internal class ErrMessageBuilder
    {
        public static string BuildByStack(DetailedError error, StackFrame where)
        {
            //is it unreadable? Don't try to read :+1:
            string column = Parser.Config.Parser.Config.Read("Column","Error");
            string errornumber =  Parser.Config.Parser.Config.Read("ErrorNumber", "Error");
            string message =  Parser.Config.Parser.Config.Read("Message", "Error");
            string errchar =  Parser.Config.Parser.Config.Read("ErrChar", "Error");
                
            string ret = $@"{$"{"/!\\".Color(column)} BH#{(int)error.ErrorPathCode}#{error.ErrorID}".Color(errornumber)} - DevCode -> {error.DevCode} | Path '{error.ErrPath} | {where.GetFileName()} | {where.GetFileLineNumber()}' 
{"|!|".Color(column)} {Color.ColorByIndex(error.ErrorMessage, 0, message)}
{"|!|".Color(column)} Ln: '{error.LineC}' | ChLn: '{error.TotalIndexOfLineWords}-{error.TotalIndexOfLineWords + error.HighLightLen}' | Ch: '{error.line.Substring(error.TotalIndexOfLineWords, error.HighLightLen).Color(errchar)}' | Time: {error.Date}  
{"\\!/".Color(column)} {Color.ColorByIndex(error.line, error.TotalIndexOfLineWords, error.HighLightLen, errchar)}
";
            return ret;
            
        }
    }
}
