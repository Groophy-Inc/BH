namespace BH.Parser.Commands
{
    public class compile
    {
        public static bool isEnd = false;
        public static int left = 0;

        public static void Init()
        {
            var com = new CommandBuilder();
            com.Create("compile",new string[]
            {
                "[$]"
            }, Decompose);
            Parse.Commands.Add(com);
        }
        
        public static int Decompose(pbp_Command command)
        {
            Runner.Run(ref command, ref left, ref isEnd);

            
            if (isEnd)
            {
                isEnd = false;

                var varName = command.Commands[0].Result.ToString().Substring(1).TrimEnd(';');
                Builder.Build.Demo(Varriables.Get(varName).Obj, false);

                Parse.EndProcess();
            }
            
            return 0;
        }
    }
}