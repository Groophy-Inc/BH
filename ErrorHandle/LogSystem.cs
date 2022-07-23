﻿using System;

namespace BH.ErrorHandle
{
    internal class LogSystem
    {
        static public bool DEBUG = false;

        static public void log(string text, ConsoleColor c = ConsoleColor.White)
        {
            if (!DEBUG) return;

            var fc = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.WriteLine(text);
            Console.ForegroundColor = fc;
        }
    }
}
