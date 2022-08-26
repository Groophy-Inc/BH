using BH.Parser.Utils;
using System.Text;
using System.Collections.Generic;
using System;
using ANSIConsole;
using BH.Structes.ErrorStack;
using BH.Parser.Config;

namespace BH.ErrorHandle.Error
{
    
    internal class ErrMessageBuilder
    {
        public static string BuildByStack(DetailedError error, string where)
        {
            //is it unreadable? Don't try to read :+1:
            return $@"{$"{"/!\\".Color("#"+Parser.Config.Parser.Config.Read("Column","Error"))} BH#{(int)error.ErrorPathCode}#{error.ErrorID}".Color("#"+Parser.Config.Parser.Config.Read("ErrorNumber","Error"))} - DevCode -> {error.DevCode} | Path '{error.ErrPath} | {where}' 
{"|!|".Color("#"+Parser.Config.Parser.Config.Read("Column","Error"))} {Color.ColorByIndex(error.ErrorMessage, 0, ("#"+Parser.Config.Parser.Config.Read("Message","Error")))}
{"|!|".Color("#"+Parser.Config.Parser.Config.Read("Column","Error"))} Ln: '{error.LineC}' | ChLn: '{error.TotalIndexOfLineWords}-{error.TotalIndexOfLineWords + error.HighLightLen}' | Ch: '{error.line.Substring(error.TotalIndexOfLineWords, error.HighLightLen).Color("#"+Parser.Config.Parser.Config.Read("ErrChar","Error"))}' | Time: {error.Date}  
{"\\!/".Color("#"+Parser.Config.Parser.Config.Read("Column","Error"))} {Color.ColorByIndex(error.line, error.TotalIndexOfLineWords, error.HighLightLen, ("#"+Parser.Config.Parser.Config.Read("ErrChar","Error")))}
";
        }
    }
}
