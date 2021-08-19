using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel
{
    public class IPAddress
    {
        public string Address { get;private set;}

        public long? PingRoundTrip { get; private set;}

        public IPAddress(string ipaddr)
        {
            Address = ipaddr;
        }

        public void Ping()
        {
            Debug.WriteLine($"PING {Address}");

            long rt = PingService.PingHost(Address);
            if (rt >0)
            {
                PingRoundTrip = rt;
                Debug.WriteLine($"PING-SUCCESS {Address} == {PingRoundTrip}");
            }
            else
            {
                 Debug.WriteLine($"PING-ERROR {Address}");

            }
                
        }
    }
}
