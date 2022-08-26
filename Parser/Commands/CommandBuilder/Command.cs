using System;
using System.Collections.Generic;

namespace BH.Parser.Commands
{
    public enum CommandParseType
    {
        signed_value,   //[ums]
        signed_values,  //[CF|PS|CS]
        unsigned_value, //[$]
        content,        //[""]
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