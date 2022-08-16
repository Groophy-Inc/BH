using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANSIConsole;

namespace BH.Parser.Utils
{
    internal class Color
    {
        static public ANSIString ColorByIndex(string text, int Index, ConsoleColor color)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index);
            var after = text.Substring(Index).Color(color);
            return new ANSIString(before + after.ToString());
        }

        static public ANSIString ColorByIndex(string text, int Index, int Len, ConsoleColor color)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index);
            var mid = text.Substring(Index, Len).Color(color);
            var after = text.Substring(Index + Len);
            return new ANSIString(before + mid.ToString() + after);
        }

        static public ANSIString ColorByIndex(string text, int Index, ConsoleColor DefualtColor, ConsoleColor color)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index).Color(DefualtColor);
            var after = text.Substring(Index).Color(color);
            return new ANSIString(before.ToString() + after.ToString());
        }

        static public ANSIString ColorByIndex(string text, int Index, int Len, ConsoleColor DefualtColor, ConsoleColor color)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index).Color(DefualtColor);
            var mid = text.Substring(Index, Len).Color(color);
            var after = text.Substring(Index + Len).Color(DefualtColor);
            return new ANSIString(before.ToString() + mid.ToString() + after.ToString());
        }

        static public ANSIString ColorByIndex(string text, int Index, string HEXcolor)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index);
            var after = text.Substring(Index).Color(HEXcolor);
            return new ANSIString(before + after.ToString());
        }

        static public ANSIString ColorByIndex(string text, int Index, string defualtHEXcolor, string HEXcolor)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index).Color(defualtHEXcolor);
            var after = text.Substring(Index).Color(HEXcolor);
            return new ANSIString(before.ToString() + after.ToString());
        }

        static public ANSIString ColorByIndex(string text, int Index, int Len, string HEXcolor)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index);
            var mid = text.Substring(Index, Len).Color(HEXcolor);
            var after = text.Substring(Index + Len);
            var retenv = new ANSIString(before + mid.ToString() + after);

            return retenv;
        }

        static public ANSIString ColorByIndex(string text, int Index, int Len, string HEXcolor, ref int Increase)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index);
            int bf = text.Substring(Index, Len).Length;
            var mid = text.Substring(Index, Len).Color(HEXcolor);
            int af = mid.ToString().Length;
            var after = text.Substring(Index + Len);
            var retenv = new ANSIString(before + mid.ToString() + after);
            Increase = af - bf;
            return retenv;
        }

        static public ANSIString ColorByIndex(string text, int Index, int Len, string defualtHEXcolor, string HEXcolor)
        {
            if (string.IsNullOrEmpty(text)) return new ANSIString("");
            var before = text.Substring(0, Index).Color(defualtHEXcolor);
            var mid = text.Substring(Index, Len).Color(HEXcolor);
            var after = text.Substring(Index + Len).Color(defualtHEXcolor);
            return new ANSIString(before.ToString() + mid.ToString() + after.ToString());
        }
    }
}
