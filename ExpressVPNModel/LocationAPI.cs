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
    internal static class LocationAPI
    {

        private static string mockAPIURL = "https://private-16d939-codingchallenge2020.apiary-mock.com/locations";

        internal static XmlDocument FetchLocations(string url=null)
        {
            if (url == null)
                url = mockAPIURL;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; ;
            request.Method = "GET";
            request.ContentType = "application/xml";

            try
            {
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
            catch (Exception ex)
            {
                //FIXME

                //Log.Error($"TimeSeriesWebRequest Exception: {ex.Message}");
               // throw ex;
            }

            return null;
        }

    }

   
}
