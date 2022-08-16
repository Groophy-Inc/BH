using BH.Parser.Utils;
using System.Text;
using System.Collections.Generic;
using System;
using ANSIConsole;

namespace BH.ErrorHandle.Error
{
    public enum ErrorPathCodes
    {
        Program = 0,
        Parser = 1
    }
    internal class ErrMessageBuilder
    {
        public static string BuildByStack(DetailedError error)
        {
            //is it unreadable? Don't try to read :+1:
            return $@"{$"BH#{(int)error.ErrorPathCode}#{error.ErrorID}".Color(ConsoleColor.Blue)} - DevCode -> {error.DevCode} | Path '{error.ErrPath}'
{Color.ColorByIndex(error.ErrorMessage, 0, ConsoleColor.Yellow)}
Ln: '{error.LineC}' | ChLn: '{error.TotalIndexOfLineWords}-{error.TotalIndexOfLineWords + error.HighLightLen}' | Ch: '{error.line.Substring(error.TotalIndexOfLineWords, error.HighLightLen).Color(ConsoleColor.Magenta)}' | Time: {error.Date}  
{Color.ColorByIndex(error.line, error.TotalIndexOfLineWords, error.HighLightLen, ConsoleColor.Red)}
";
        }
    }
}
