using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExpressVPNClientModel
{
    internal class XMLWebRequestProcessor
    {

        public XmlDocument Response { get; private set; }

        public Exception RequestException { get; private set; }

        internal XMLWebRequestProcessor(string url)
        {
            try
            {
                Response = FetchAndProcess(url);
            }
            catch (Exception ex)
            {
                RequestException = ex;
            }
        }

        private XmlDocument FetchAndProcess(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("URL");

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/xml";


            string rawxml;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                rawxml = reader.ReadToEnd().Replace("\n", "");
            }

            if (string.IsNullOrEmpty(rawxml))
                throw new Exception("Null response from web request");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rawxml);
            return doc;

        }

    }

}



