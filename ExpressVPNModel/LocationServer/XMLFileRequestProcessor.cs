using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace ExpressVPNClientModel.LocationServer
{
    public class XMLFileRequestProcessor : IRequestProcessor
    {

        public const string GenericExceptionStr = "An unknown error happened";

        public Exception RequestException { get;private set;}

        public XmlDocument Process(string uri, WebRequestFactory factory = null)
        {
            //Uri is expected to be file://{qualified path}

            try
            {
                if (string.IsNullOrEmpty(uri))
                    throw new Exception(GenericExceptionStr);

                if (!uri.ToLower().StartsWith("file://"))
                    throw new Exception(GenericExceptionStr);

                string filename = uri.Replace("file://","");


                var xml = File.ReadAllText(filename);

                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                return doc;
            }

            catch(Exception ex)
            {
                RequestException = ex;
            }

            return null;
        }
    }
}
