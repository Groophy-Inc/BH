using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Structes.BodyClasses
{
    public class _Namespace
    {
        public string Name { get; set; } = String.Empty;
        public List<_Class> Classes { get; set; } = new List<_Class>();
        public List<string> Nugets { get; set; } = new List<string>();
        public List<string> Using { get; set; } = new List<string>();
    }

    public class _Class
    {
        public string Name { get; set; }
        public string Access { get; set; } = "public";
        public List<_Void> Voides { get; set; } = new List<_Void>();
        public bool isConjunction { get; set; } = false;
        public string Conjunction { get; set; }
    }

    public class _Void
    {
        public string Access { get; set; } = "public";
        public List<string> Args { get; set; } = new List<string>();
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool isField { get; set; } = false;
        public string ReturnType { get; set; } = "void";
        public string FieldDefualt { get; set; } = string.Empty;
    }
}
