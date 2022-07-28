using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BH.Server
{
    internal class Handle
    {
        public static void Inıt()
        {
            Console.Title = "Server";
            // Create a Http server and start listening for incoming connections
            SimpleListenerExample(new string[] { "http://localhost:1756/" });

            //very beta
        }

        public static void SimpleListenerExample(string[] prefixes)
        {
            while (true)
            {
                if (!HttpListener.IsSupported)
                {
                    Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                    return;
                }
                // URI prefixes are required,
                // for example "http://contoso.com:8080/index/".
                if (prefixes == null || prefixes.Length == 0)
                    throw new ArgumentException("prefixes");

                // Create a listener.
                HttpListener listener = new HttpListener();
                // Add the prefixes.
                foreach (string s in prefixes)
                {
                    listener.Prefixes.Add(s);
                }
                listener.Start();
                Console.WriteLine("Listening...");
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                var queue = request.QueryString;
                HttpListenerResponse response = context.Response;
                string responseString = "<HTML><BODY> Error</BODY></HTML>";

                foreach (string s in queue)
                {
                    if (s == "parse")
                    {
                        Parser.Parse.ParseMasterPage("", queue[s], new string[] { });
                        responseString = Parser.Parse.logErrMsg;
                    }
                }

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                listener.Stop();
            }
        }
    }
}
