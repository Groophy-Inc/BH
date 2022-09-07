using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BH.ErrorHandle;
using BH.Parser.Commands.SubCommand;

namespace BH.Parser.Commands
{
    
    public class app
    {
        public static bool isEnd = false;
        public static int left = 0;

        public static void Init()
        {
            var com = new CommandBuilder();
            com.Create("app",new string[]
            {
                "[new]",
                "[comp]",
                "[->]",
                "[$]",
                "[as]",
                "[?()]",
                "[:]",
                "[\":\"]"
            }, Decompose);
            Parse.Commands.Add(com);
        }
        
        public static int Decompose(pbp_Command command)
        {
            Runner.Run(ref command, ref left, ref isEnd);

            
            
            if (isEnd)
            {
                isEnd = false;

                Comp.Update(command.Commands[3].Result.ToString(), (string)command.Commands[5].Result, ((attributes)command.Commands[7].Result));

                Parse.EndProcess();
                left = 0;
            }

           
            
            return 0;
        }
    }
}