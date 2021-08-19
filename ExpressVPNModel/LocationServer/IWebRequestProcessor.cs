using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExpressVPNClientModel.LocationServer
{
    public interface IWebRequestProcessor
    {
        void Process(string url, WebRequestFactory factory = null);

        XmlDocument ResponseXml { get; }

        Exception RequestException { get; }


    }
}
