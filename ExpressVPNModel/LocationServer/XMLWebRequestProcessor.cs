using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExpressVPNClientModel.LocationServer
{
    public class XMLWebRequestProcessor : IWebRequestProcessor
    {
        public const string ForbiddenExceptionStr = "The API returned error code 403";
        public const string GenericExceptionStr = "An unknown error happened";
        public const string TimeoutExceptionStr = "The request timed out";

        public XmlDocument ResponseXml { get; private set; }

        public Exception RequestException { get; private set; }

        public void Process(string url, WebRequestFactory factory = null)
        {
            ResponseXml=null;
            RequestException=null;

            if (factory == null)
                factory = new WebRequestFactory();

            try
            {
                ResponseXml = FetchAndProcess(url, factory);
            }
            catch (Exception ex)
            {
                RequestException = ex;
            }
        }



        private XmlDocument FetchAndProcess(string url, WebRequestFactory factory)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("URL");

            if (factory == null)
                throw new ArgumentNullException("WebRequestFactory");

            IHttpWebRequest request = factory.Create(url, "GET", "application/xml");

            string rawxml;
            IHttpWebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                // If we got here, it was a timeout exception.
                throw new Exception(TimeoutExceptionStr);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                //No response / connection error / bad uRL etc
                throw new Exception(GenericExceptionStr);
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new Exception(ForbiddenExceptionStr);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(GenericExceptionStr);

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                rawxml = reader.ReadToEnd().Replace("\n", "");
            }

            if (string.IsNullOrEmpty(rawxml))
                throw new Exception(GenericExceptionStr);

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(rawxml);
                return doc;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception(GenericExceptionStr);
            }

        }

    }

}




