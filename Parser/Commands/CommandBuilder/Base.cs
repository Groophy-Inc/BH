using System;

namespace BH.Parser.Commands
{
    public class CommandBuilder
    {
        public pbp_Command Registery { get; set; } = new pbp_Command();

        public void Create(string Commandname,string[] registery, Func<pbp_Command, int> Parser)
        {
            pbp_Command command = new pbp_Command();
            command.CommandName = Commandname;

            foreach (var reg in registery)
            {
                Command c = new Command();
                string clear = reg.TrimStart('[').TrimEnd(']');

                bool isSigned_Values = (clear.IndexOf('|') == -1) ? false : true;

                if (isSigned_Values) //Signed_Values
                {
                    c.Type = CommandParseType.signed_values;
                    string[] values = clear.Split('|');
                    c.Value = values;
                }
                else if (clear.StartsWith('$')) //unsigned_value
                {
                    c.Type = CommandParseType.unsigned_value;
                }
                else if (clear.StartsWith("\"\"")) //Content
                {
                    c.Type = CommandParseType.content;
                }
                else if (clear.StartsWith("\":\"")) //attribute
                {
                    c.Type = CommandParseType.attribute;
                    c.StatusValue_NotForUsers = -1;
                }
                else //Signed_Value
                {
                    c.Type = CommandParseType.signed_value;
                    c.Value = clear;
                }
                
                command.Commands.Add(c);
            }

            command.Delegate = Parser;
            Registery = command;
        }
    }
}