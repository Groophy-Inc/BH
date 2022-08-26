using System;
using System.Linq;
using BH.ErrorHandle;

namespace BH.Parser.Commands
{
    public class printdbg
    {
        public static bool isEnd = false;
        public static int left = 0;

        public static void Init()
        {
            var com = new CommandBuilder();
            com.Create("printdbg",new string[]
            {
                "[\"\"]"
            }, Decompose);
            Parse.Commands.Add(com);
        }
        
        public static int Decompose(pbp_Command command)
        {
            Runner.Run(ref command, ref left, ref isEnd);

            
            if (isEnd)
            {
                isEnd = false;

                string log = Varriables.FixContent(command.Commands[0].Result.ToString());
                Logs.Log(log);
                
                Console.WriteLine(log);
                
                Parse.EndProcess();
            }
            
            return 0;
        }
    }
}