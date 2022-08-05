using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Parser
{
    internal class Keys
    {
        public static string[] getKeyWordsAsArray()
        {
            List<string> KeyWordList = new List<string>();
            foreach (KeyWords val in Enum.GetValues(typeof(KeyWords)))
            {
                KeyWordList.Add(val.ToString());
            }
            return KeyWordList.ToArray();
        }

        public enum KeyWords
        {
            set,
            msg,
            system,
            ums,
            var
        }
    }
}
