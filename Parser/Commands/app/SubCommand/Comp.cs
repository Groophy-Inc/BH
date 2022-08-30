using System;
using System.Collections.Generic;
using System.Linq;
using BH.Structes;
using BH.Structes.BodyClasses;

namespace BH.Parser.Commands.SubCommand
{
    public class Comp
    {
        public static void Update(string Varriable, string Component, attributes attr)
        {
            bool isFound = false;
            Element ele = (Element)Varriables.TryGet(Varriable, ref isFound).Obj;
            
            var list = ele.comps;
            
            list.Add(new Tuple<string, string, Dictionary<string, string>>(Component, TryGetOrDefualt(attr, "inner", ""), attr.getDic()));
            
            ele.comps = list;

            Varriables.AddorUpdate(Varriable, ele);
        }

        public static string TryGetOrDefualt(attributes attr, string key, string defualt)
        {
            if (attr.attr.Any(x => x.key == key))
            {
                return attr.attr.Where(x => x.key == key).First().value;
            }
            else return defualt;
        }
    }
}