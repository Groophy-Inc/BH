using System;
using ANSIConsole;

namespace BH.ANSIIConsole
{
    internal class Gecho
    {
        public static void Print(string text)
        {
            Coologs c = new Coologs(false);
            c.Print(text + "\r\n");
            Console.ResetColor();
        }
    }

    internal class Coologs
    {
        public Coologs(bool logmode = false)
        {
            isLog = logmode;

        }

        public static bool isLog = false;
        /// <summary>
        /// Prints a text and applies foreground changes with &lt;colors&gt; tags in it.
        /// </summary>
        /// <param name="text_code">Text desired with print styling code</param>
        /// <param name="endwith">Character at the end of the print -- default is a line break</param>
        /// <example>Coologs.Print("&lt;red&gt;Red text<blue> Blue text</blue> Normal text");</example>
        /// <remarks>You can escape the character '&lt;' with '&lt;' (Example: '&lt;&lt;test&gt;'</remarks>
        public string Print(string text_code, string endwith = "\r\n")
        {
            string log = string.Empty;
            bool parsing_tag = false;
            string tag = "";
            bool isPastel = false;
            bool isPastelBg = false;
            string lastesthex = string.Empty;
            string lastesthexbg = string.Empty;
            bool isBold = false; //dolgulu
            bool isItalic = false; //eğik
            bool isUnderlined = false; //altı çizgili
            bool isInverted = false; //ters
            bool isStrikeThrough = false; //ortadan çizgili
            bool isOverlined = false; //üstü çizgili
            bool isBackSlash = false;

            bool isHyperLink = false; //clickable link for new console
            string LastestHyperLink = string.Empty;


            int Opacity = 100;

            for (int i = 0; i < text_code.Length; i++)
            {
                if (text_code[i] == '\\')
                {
                    isBackSlash = true;
                    continue;
                }

                if (parsing_tag && !isBackSlash)
                {

                    if (text_code[i] == '>')
                    {
                        parsing_tag = false;
                        tag = tag.ToLower();
                        tag = tag.Substring(1);

                        if (tag.Length == 0)
                        {
                            continue;
                        }

                        //  Check for escape character '<'
                        if (tag[0] == '<')
                        {
                            Console_.Write(tag + '>');   //  We rebuild inital escaped tag because we previously remove the last '>'
                            tag = "";
                            continue;
                        }

                        //  A </> closure tag resets color
                        if (tag[0] == '/')
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            isPastel = false;
                            lastesthex = "#FFFFFF";
                        }
                        else if (tag == "bg/")
                        {
                            isPastelBg = false;
                            lastesthexbg = "";
                        }
                        else if (tag == "-/")
                        {
                            Opacity = 100;
                        }
                        //  Sets desired color
                        else
                        {
                            if (tag.StartsWith("bg"))
                            {
                                string withoutbg = tag.Substring(2);
                                if (withoutbg.StartsWith("#"))
                                {
                                    isPastelBg = true;
                                    lastesthexbg = withoutbg;
                                }
                                else
                                {

                                    //  If the color code is incorrect, we don't raise any error -- just let it go
                                    string fullName = getColorHex(withoutbg);

                                    isPastelBg = true;
                                    lastesthexbg = "#" + fullName;
                                }
                            }
                            else if (tag.StartsWith("+"))
                            {
                                char prefix = tag[1];
                                if (prefix == '*')
                                {
                                    isBold = true;
                                }
                                else if (prefix == '~')
                                {
                                    isItalic = true;
                                }
                                else if (prefix == '_')
                                {
                                    isUnderlined = true;
                                }
                                else if (prefix == ':')
                                {
                                    isInverted = true;
                                }
                                else if (prefix == '-')
                                {
                                    isStrikeThrough = true;
                                }
                                else if (prefix == '^')
                                {
                                    isOverlined = true;
                                }
                                else if (prefix == '$')
                                {
                                    string link = tag.Substring(2);

                                    isHyperLink = true;
                                    LastestHyperLink = link;
                                }
                            }
                            else if (tag.StartsWith("!"))
                            {
                                char prefix = tag[1];
                                if (prefix == '*')
                                {
                                    isBold = false;
                                }
                                else if (prefix == '~')
                                {
                                    isItalic = false;
                                }
                                else if (prefix == '_')
                                {
                                    isUnderlined = false;
                                }
                                else if (prefix == ':')
                                {
                                    isInverted = false;
                                }
                                else if (prefix == '-')
                                {
                                    isStrikeThrough = false;
                                }
                                else if (prefix == '^')
                                {
                                    isOverlined = false;
                                }
                                else if (prefix == '$')
                                {
                                    isHyperLink = false;
                                    LastestHyperLink = string.Empty;
                                }
                            }
                            else
                            {
                                string lastesttag = tag;

                                string opac = "";
                                if (lastesttag.StartsWith("-"))
                                {
                                    lastesttag = lastesttag.Substring(1);
                                    for (int t = 0; t < 3; t++)
                                    {
                                        if (lastesttag[t] == '-')
                                        {
                                            break;
                                        }
                                        opac += lastesttag[t].ToString();
                                    }

                                    Opacity = Convert.ToInt32(opac);
                                    lastesttag = lastesttag.Substring(opac.Length + 1);
                                }

                                if (lastesttag.StartsWith("#"))
                                {
                                    isPastel = true;
                                    lastesthex = lastesttag;
                                }
                                else
                                {

                                    //  If the color code is incorrect, we don't raise any error -- just let it go
                                    lastesthex = "#" + getColorHex(lastesttag);
                                    isPastel = true;

                                }
                            }

                        }

                        tag = "";
                        continue;
                    }

                    tag += text_code[i];
                    continue;
                }

                if (text_code[i] == '<' && !isBackSlash)
                {
                    parsing_tag = true;
                    tag += "<";
                    continue;
                }


                ANSIString text = text_code[i].ToString().Opacity(Opacity);

                if (isPastel) text = text.Color(lastesthex);

                if (isPastelBg) text = text.Background(lastesthexbg);

                if (isBold) text = text.Bold();

                if (isItalic) text = text.Italic();

                if (isUnderlined) text = text.Underlined();

                if (isInverted) text = text.Inverted();

                if (isStrikeThrough) text = text.StrikeThrough();

                if (isOverlined) text = text.Overlined();

                if (isHyperLink) text = text.SetHyperlink(LastestHyperLink);

                Console_.Write(text);

                if (isBackSlash) isBackSlash = false;

                if (isLog) Console_.WriteLine("\r\nText: " + text_code[i] + ", IsPastel: " + isPastel.ToString() + ", hex: " + lastesthex + ", hexbg: " + lastesthexbg + ", opacity: " + Opacity + ", tag: " + tag + ", FullText: " + text);

                log += "\r\nText: " + text_code[i] + ", IsPastel: " + isPastel.ToString() + ", hex: " + lastesthex + ", hexbg: " + lastesthexbg + ", opacity: " + Opacity + ", tag: " + tag + ", FullText: " + text + "\r\n";

            }

            //Console_.Write(endwith);
            return log;
        }
        /// <summary>
        /// Returns color's int code based on its string representation
        /// </summary>
        /// <param name="code">Color's string code</param>
        /// <remarks>Returns -1 if the input doesn't match any color.</remarks>
        private static string getColor(string code) // b -> blue
        {
            if (code == "b") code = "blue";
            else if (code == "c") code = "cyan";
            else if (code == "bk") code = "black";
            else if (code == "db") code = "darkblue";
            else if (code == "dc") code = "darkcyan";
            else if (code == "dgn") code = "darkgreen";
            else if (code == "dgy") code = "darkgray";
            else if (code == "dm") code = "darkmagenta";
            else if (code == "dr") code = "darkred";
            else if (code == "dy") code = "darkyellow";
            else if (code == "gy") code = "gray";
            else if (code == "gn") code = "green";
            else if (code == "m") code = "magenta";
            else if (code == "r") code = "red";
            else if (code == "w") code = "white";
            else if (code == "y") code = "yellow";

            foreach (int i in Enum.GetValues(typeof(ConsoleColor)))
            {
                if (((ConsoleColor)i).ToString().ToLower() == code) //max 15
                {
                    return i.ToString();
                }
            }

            return "-1";
        }


        private static string gethexbycode(string code) //blue -> 0000FF
        {
            if (code == "blue") return "0000FF";
            else if (code == "cyan") return "00FFFF";
            else if (code == "black") return "000000";
            else if (code == "darkblue") return "00008B";
            else if (code == "darkcyan") return "008080";
            else if (code == "darkgreen") return "006400";
            else if (code == "darkgray") return "A9A9A9";
            else if (code == "darkmagenta") return "8B008B";
            else if (code == "darkred") return "8B0000";
            else if (code == "darkyellow") return "F6BE00";
            else if (code == "gray") return "808080";
            else if (code == "green") return "008000";
            else if (code == "magenta") return "FF00FF";
            else if (code == "red") return "FF0000";
            else if (code == "white") return "FFFFFF";
            else if (code == "yellow") return "FFFF00";
            else return "000000";
        }

        private static string getColorHex(string code) //b -> blue -> 0000FF
        {
            code = code.ToLower();
            if (code == "b") code = "blue";
            else if (code == "c") code = "cyan";
            else if (code == "bk") code = "black";
            else if (code == "db") code = "darkblue";
            else if (code == "dc") code = "darkcyan";
            else if (code == "dgn") code = "darkgreen";
            else if (code == "dgy") code = "darkgray";
            else if (code == "dm") code = "darkmagenta";
            else if (code == "dr") code = "darkred";
            else if (code == "dy") code = "darkyellow";
            else if (code == "gy") code = "gray";
            else if (code == "gn") code = "green";
            else if (code == "m") code = "magenta";
            else if (code == "r") code = "red";
            else if (code == "w") code = "white";
            else if (code == "y") code = "yellow";

            return gethexbycode(code);
        }
    }
}
