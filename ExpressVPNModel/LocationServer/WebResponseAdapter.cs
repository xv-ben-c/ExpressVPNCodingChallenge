using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel.LocationServer
{
    public interface IHttpWebResponse
    {
        HttpStatusCode StatusCode { get; set; }
        Stream GetResponseStream();
    }


    internal class StandardHttpWebResponse : IHttpWebResponse
    {
        private HttpWebResponse Response;

        internal StandardHttpWebResponse(HttpWebResponse resp)
        {
            Response = resp;  
        }

        public HttpStatusCode StatusCode
        {
            get { return Response.StatusCode;}
            set { throw new NotImplementedException();}
        }

        public Stream GetResponseStream()
        {
            return Response.GetResponseStream();
        }
    }

}
