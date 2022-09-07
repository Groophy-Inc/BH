using System;
using System.Collections.Generic;

namespace BH.Parser.Commands
{
    public enum CommandParseType
    {
        signed_value,   //[ums]
        signed_values,  //[CF|PS|CS]
        unsigned_varriable, //[$]
        content,        //[""]
        attribute,      //[":"]
        unsigned_value  //[?()]
    }

    public class attributes
    {
        public List<attribute> attr { get; set; } = new List<attribute>();

        public attribute lastest_attr { get; set; } = new attribute();

        public Dictionary<string, string> getDic()
        {
            Dictionary<string, string> retenv = new Dictionary<string, string>();
            foreach (var attribute in attr)
            {
                if (attribute.key.Replace('I', 'i').ToLower() != "inner") retenv.Add(attribute.key, attribute.value);
            }

            return retenv;
        }
    }
    
    public class attribute
    {
        public string key { get; set; }
        public string value { set; get; }

        public void appentToContent(string text, int status)
        {
            if (status == 1) key += text;
            else if (status == 4) value += text;
        }
    }

    public class pbp_Command
    {
        public string CommandName { get; set; } = "NaN";
        public List<Command> Commands { get; set; } = new List<Command>();
        public Func<pbp_Command,int> Delegate { get; set; }
    }
    
    public class Command
    {
        public CommandParseType Type { get; set; }
        public object Value { get; set; }
        public int StatusValue_NotForUsers { get; set; } = 0;
        public object Result { get; set; }
    }
}