namespace BH
{
    internal class Program
    {
        static async System.Threading.Tasks.Task Main()
        {
            System.Func<int> MainWorker = delegate()
            {
                Console_.Write(APF.Helper.FixRead(System.IO.File.ReadAllText(BH.Parser.Parse.masterPagePath)+"\r\n-------------------------"));
                return 0;
            };

            var _Runner = await Runner.Base.Run(false);
        }
    }
}
