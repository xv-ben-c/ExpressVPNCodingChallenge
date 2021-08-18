using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ExpressVPNClientModel.LocationServer
{

    public class WebRequestFactory
    {

        public virtual IHttpWebRequest Create(string url, string method, string contentType)
        {
            var standardRequest = new StandardHttpWebRequest(url, 10000)
            {
                ContentType = contentType,
                Method = method
            };

            return standardRequest;
        }

    }

   

}
