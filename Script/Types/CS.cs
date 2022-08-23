using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CSScriptLib;

namespace BH.Script.Types
{
    public struct CSGrp
    {
        public string Stdin;
        public object Stdout;
        public bool Stderr;
        public Stopwatch Stopwatch;
        public bool isTimeout;
    }
    
    public class CS
    {
        private static string LastestScript = "";
        private static Stopwatch LTsw = new Stopwatch();
        public static object Execute(string script, int timeoutMS = 3000)
        {
            LastestScript = script;
            CSGrp cs = new CSGrp();
            cs.Stdin = script;
            Task<object> task = Task.Run(() =>
            {
                try
                {
                    LTsw = Stopwatch.StartNew();
                    dynamic script = CSScript.RoslynEvaluator
                        .LoadMethod(LastestScript);
                    var ret = script.Main();
                    LTsw.Stop();
                    return ret;
                }
                catch
                {
                    return "Stderr";
                }
               
            });
            
            if (task.Wait(TimeSpan.FromMilliseconds(timeoutMS)))
            {
                cs.Stdout = task.Result;
                cs.Stopwatch = LTsw;
            }
            else
            {
                cs.Stdout = "Timeout.";
                cs.isTimeout = true;
                cs.Stopwatch = LTsw;
            }

            if (task.Result.ToString() == "Stderr") cs.Stderr = true;

            LastestScript = "";
            return cs;
        }
    }
}