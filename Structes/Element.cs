using System.Collections.Generic;

namespace BH.Structes
{
    public class Element
    {
        public string Genre { get; set; }
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
    }
}
