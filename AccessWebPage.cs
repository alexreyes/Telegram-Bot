using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Api_Practice
{
    class AccessWebPage
    {
        public static string HttpGet(string url)
        {
            //HttpWebRequest provides an HTTP-specific implementation of the WebRequest class
            //WebRequest makes a request to a Uniform Resource Identifier (URI). This is an abstract class.
            //this is like using the new command but creating the instance using an abstract and making it specific
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            string result = null;
            try
            {

                using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(resp.GetResponseStream());
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception) { }
            return result;
        }
    }
}