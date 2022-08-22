using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BH.APF
{
    internal class ParalelPrint //TODO
    {
        public static Dictionary<Point, char> table = new Dictionary<Point, char>();

        private static int MaxX, MaxY = 0;
        public static void Inıt(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    table.TryAdd(new Point(x, y), ' ');
                }
            }
        }

        public static void Insert(int x, int y, char c)
        {
            table[new Point(x, y)] = c;
        }

        public static void Print(string first, string second)
        {
            ClearCurrentConsoleLine(0);
            ClearCurrentConsoleLine(1);
            ClearCurrentConsoleLine(2);
            int half = Console.WindowWidth / 2;

            half = (half % 2 == 0) ? half : half + 1;

            Console.SetBufferSize(Console.WindowWidth ,(first.Length >= second.Length) ? first.Length : second.Length);

            int ten = 0;
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                if (i % 10 == 0 && i != 0)
                {
                    WriteCharAt(i, 1, (i / 10).ToString());
                    
                    ten = 0;
                }
                WriteCharAt(i, 0, ten.ToString());
                ten++;
            }
            Console.SetCursorPosition(0, 3);
            
        }

        private static void PrintTable()
        {
            Console.Clear();
            for (int x = 0; x < MaxX; x++)
            {
                for (int y = 0; y < MaxY; y++)
                {
                    Console.Write(table[new Point(x, y)]);
                }
            }
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadConsoleOutputCharacter(
            IntPtr hConsoleOutput,
            [Out] StringBuilder lpCharacter,
            uint length,
            COORD bufferCoord,
            out uint lpNumberOfCharactersRead);

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }

        public static void IncreaseY()
        {
            Console.Write("\r\n");
            //Console.SetCursorPosition(0, Console.WindowHeight+1);
        }

        public static char ReadCharacterAt(int x, int y)
        {
            IntPtr consoleHandle = GetStdHandle(-11);
            if (consoleHandle == IntPtr.Zero)
            {
                return '\0';
            }
            COORD position = new COORD
            {
                X = (short)x,
                Y = (short)y
            };
            StringBuilder result = new StringBuilder(1);
            uint read = 0;
            if (ReadConsoleOutputCharacter(consoleHandle, result, 1, position, out read))
            {
                return result[0];
            }
            else
            {
                return '\0';
            }
        }

        public static char[] ReadLineAt(int Y)
        {
            int len = Console.WindowWidth;
            char[] result = new char[len];

            for (int x = 0; x < len; x++)
            {
                result[x] = ReadCharacterAt(x, Y);
            }
            return result;
        }

        public static int[] IReadLineAt(int Y)
        {
            int len = Console.WindowWidth;
            int[] result = new int[len];

            for (int x = 0; x < len; x++)
            {
                result[x] = (int)ReadCharacterAt(x, Y);
            }
            return result;
        }

        public static void RemoveCharAt(int x, int y)
        {
            int envTop = Console.CursorTop;
            int envLeft = Console.CursorLeft;
            Console.SetCursorPosition(x, y);
            Console.Write(' ');
            Console.SetCursorPosition(envLeft, envTop);
        }

        public static void RemoveLineAt(int y)
        {
            int envTop = Console.CursorTop;
            int envLeft = Console.CursorLeft;
            Console.SetCursorPosition(0, y);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(envLeft, envTop);
        }

        public static void WriteCharAt(int x, int y, string c)
        {
            int envTop = Console.CursorTop;
            int envLeft = Console.CursorLeft;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            Console.SetCursorPosition(envLeft, envTop);
        }
        public static void WriteCharAt(int x, int y, char c)
        {
            int envTop = Console.CursorTop;
            int envLeft = Console.CursorLeft;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            Console.SetCursorPosition(envLeft, envTop);
        }

        public static void ClearCurrentConsoleLine(int DelLineCount = 0)
        {
            for (int i = 1; i < DelLineCount; i++)
            {
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop - i);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }
            Console.SetCursorPosition(0, Console.CursorTop - DelLineCount + 1);
        }
    }
}
