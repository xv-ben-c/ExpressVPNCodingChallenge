using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel.LocationServer
{
    public interface IHttpWebRequest
    {
        string Method { get; set; }
        string ContentType { get; set; }

        IHttpWebResponse GetResponse();
    }


    internal class StandardHttpWebRequest : IHttpWebRequest
    {
        private HttpWebRequest Request;

        internal StandardHttpWebRequest(string url, int? timeout)
        {
            Request = WebRequest.Create(url) as HttpWebRequest;

            if (timeout.HasValue)
                Request.Timeout = timeout.Value;
        }

        public string Method
        {
            get { return Request.Method; }
            set { Request.Method = value; }
        }

        public string ContentType
        {
            get { return Request.ContentType; }
            set { Request.ContentType = value; }
        }

        public IHttpWebResponse GetResponse()
        {
            return new StandardHttpWebResponse( Request.GetResponse() as HttpWebResponse);
        }


    }

}
