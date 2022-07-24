using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.APF
{
    internal class Helper
    {
        public static int BackslashCount(string src)
        {
            return (src.Split(new[] { '\'', '\"', '\\', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v'  }, StringSplitOptions.None)).Length - 1;
        }
    }
}
