using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel
{
    public class IPAddress
    {
        public string Address { get;private set;}

        public int? PingRoundTrip { get; private set;}

        public IPAddress(string ipaddr)
        {
            Address = ipaddr;
        }
    }
}
