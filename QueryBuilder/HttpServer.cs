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
        /// <summary>
        /// Handle all incoming connections
        /// </summary>
        /// <param name="listener">HttpListener instance</param>
        /// <returns></returns>
        public static async Task HandleIncomingConnections(HttpListener listener)
        {
            PageCreator page = new();
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

                // If requested data from MongoDb using post method
                if ((req is not null)
                    && (req.HttpMethod == "POST")
                    && (req.Url.AbsolutePath == "/getdata")
                    && req.HasEntityBody)
                {
                    // Get body of post request
                    string jsonbody;
                    using (System.IO.Stream body = req.InputStream)
                    {
                        using (var reader = new System.IO.StreamReader(body, req.ContentEncoding))
                        {
                            jsonbody = reader.ReadToEnd();
                        }
                    }
                    Console.WriteLine("Post method for URL /getdata. Body: ", jsonbody);

                    // Get data from MongoDb
                    MongoDriver driver = new();
                    string dataFromMongo = driver.GetData(jsonbody);
                    Console.WriteLine(dataFromMongo);

                    byte[] requestedData = Encoding.UTF8.GetBytes(dataFromMongo);
                    resp.ContentType = "text/html";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = requestedData.LongLength;

                    // Write out to the response stream (asynchronously), then close it
                    await resp.OutputStream.WriteAsync(requestedData, 0, requestedData.Length);
                    resp.Close();
                    continue;
                }

                // Write the response info
                string pageData = page.GetPage();
                byte[] data = Encoding.UTF8.GetBytes(pageData);
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