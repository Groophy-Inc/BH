using System.Collections.Generic;
using System.Diagnostics;

namespace BH.Structes
{
    public class Element
    {
        public string Genre { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
    }
}
