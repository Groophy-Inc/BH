using System;
using System.Collections.Generic;
using System.Diagnostics;
using BH.Structes.BodyClasses;

namespace BH.Structes
{
    public class Element
    {
        public string Genre { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public Structes.BodyClasses._Namespace appcs { get; set; } = new _Namespace();

        public List<Tuple< string,string, Dictionary<string, string>>> comps { get; set; } =
            new List<Tuple<string,string, Dictionary<string, string>>>();
    }
}
