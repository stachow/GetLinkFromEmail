using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;


namespace GetLink
{
    public class SimpleServer
    {     
        public static void Start(string prefix, Action<string> feedback, Func<string, string> getResponseGivenQuerystringDelegate)
        {
            if (!HttpListener.IsSupported)
            {
                feedback("HttpListener is not supported on your version of Windows.");
                return;
            }
            // URI prefixes are required, 
            // for example "http://contoso.com:8080/index/".
            if (prefix == null )
                throw new ArgumentException("prefix");

            // Create a listener.
            var listener = new HttpListener();
            listener.Prefixes.Add(prefix);
            listener.Start();
            while (true)
            {
                
                feedback("Listening on " + prefix);
                // Note: The GetContext method blocks while waiting for a request.
                var context = listener.GetContextAsync().Result;
               
                var request = context.Request;
                var url = request.RawUrl;

                feedback("Received: " + url);

                if (!IsInterestingUrl(url)) {
                    feedback("Not interested in " + request.RawUrl);

                    var response = context.Response;

                    response.StatusCode = 404;
                    response.ContentLength64 = 0;
                    response.OutputStream.Close();
                } 
                else
                {
                    feedback("Received " + request.RawUrl);
                    var queryString = request.Url.Query.Trim('?');

                    var responseStr = getResponseGivenQuerystringDelegate(queryString);

                    var response = context.Response;
                  
                    var responseString = responseStr;
                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    
                    response.ContentLength64 = buffer.Length;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                    
                }
            }
            //listener.Stop();
        }

        private static bool IsInterestingUrl(string url)
        {
            return !url.EndsWith(".ico");
        }
    }
}
