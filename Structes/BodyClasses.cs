using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Structes.BodyClasses
{
    public class _Namespace
    {
        public string Name { get; set; }
        public List<_Class> Classes { get; set; }
        public List<string> Nugets { get; set; }
        public List<string> Using { get; set; }
    }

    public class _Class
    {
        public string Name { get; set; }
        public List<_Void> Voides { get; set; }
    }

    public class _Void
    {
        public string Access { get; set; }
        public List<string> Args { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool isField { get; set; } = false;
        public string ReturnType { get; set; } = "void";
        public string FieldDefualt { get; set; }
    }
}
