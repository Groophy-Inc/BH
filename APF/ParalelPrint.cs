using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BH.APF
{
    internal class ParalelPrint //TODO
    {

        public static string[] _Text { get; set; } 

        public static void Inıt(int maxX, int maxY)
        {
            string[] strings = new string[maxY];
            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = new string(' ', maxX);
            }
            _Text = strings;
        }

        public static void Insert(int x, int y, char c, int cc)
        {
            try
            {
                if (_Text[y].Length <= x)
                {
                    _Text[y] = _Text[y].Insert(x, c.ToString());
                }
                else
                {
                    _Text[y] = _Text[y].Remove(x, 1).Insert(x, c.ToString());
                }
            }
            catch
            {
                Console.WriteLine(Get());
                Environment.Exit(1);
            }
        }

        public static string Get()
        {
            return string.Join('\n', _Text);
        }

        public static void Print(string first, string second)
        {
            Inıt(Console.WindowWidth, Console.WindowHeight+1);
            Console.WriteLine("Paralel Print Test");
            int Y = Console.CursorTop;
            int p1 = 0;
            int p2 = 0;

            int left = Console.WindowWidth % 2;
            if (left == 0)
            {
                p1 = Console.WindowWidth / 2;
                p2 = Console.WindowWidth / 2 + 1;
            }
            else
            {
                p1 = Console.WindowWidth / 2;
                p2 = Console.WindowWidth / 2;
            }

            int fc = Math.Max(first.Split('\n').Length, second.Split('\n').Length);
            Console.SetBufferSize(Console.WindowWidth, fc + 50);

            for (int i = 0; i < fc; i++)
            {
                Insert(p1, Y + i, '|', 0);
                IncreaseY();
            }

            int newLine = 0;
            int index = 0;
            for (int i = 0;i < first.Length;i++)
            {
                
                float leftMod = (index % (p1 - 1));
                if (leftMod == 0 && index != 0)
                {
                    newLine++;
                    index = 0;
                }
                else
                {
                    char c = first[i];
                    Insert(index, Y + newLine, c, 1);
                    index++;
                    if (c == '\n')
                    {
                        newLine++;
                        index = 0;
                    }
                }
            }

            newLine = 0;
            index = p2;
            for (int i = 0; i < second.Length; i++)
            {

                float leftMod = (index % (p1 - 1));
                if (leftMod == 0 && index != 0)
                {
                    newLine++;
                    index = p2;
                }
                else
                {
                    char c = second[i];
                    Insert(index, Y + newLine, c, 2);
                    index++;
                    if (c == '\n')
                    {
                        newLine++;
                        index = p2;
                    }
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
