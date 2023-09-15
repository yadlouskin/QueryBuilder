using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace QueryBuilder
{
    /// <summary>
    /// Simple HTTP Server using HttpListener
    /// Entry point to Query Builder application
    /// </summary>
    class HttpServer
    {
        public static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>Query Builder</title>" +
            "  </head>" +
            "  <body>" +
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {0}>" +
            "    </form>" +
            "  </body>" +
            "</html>";

        /// <summary>
        /// Handle all incoming connections
        /// </summary>
        /// <param name="listener">HttpListener instance</param>
        /// <returns></returns>
        public static async Task HandleIncomingConnections(HttpListener listener)
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine(req!.Url!.ToString());
                Console.WriteLine(req!.HttpMethod);
                Console.WriteLine(req!.UserHostName);
                Console.WriteLine(req!.UserAgent);
                Console.WriteLine();

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req is not null) && (req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                // Write the response info
                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        /// <summary>
        /// Entry point to Query Builder application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Create a Http server and start listening for incoming connections
            HttpListener listener = new HttpListener();
            string url = Configuration.GetConfiguration().QueryBuilderUrl;
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections(listener);
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }
    }
}