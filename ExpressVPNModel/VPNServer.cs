using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel
{
    public class VPNServer
    {

        public string IPAddress {get; private set; }


        internal VPNServer(string ipaddress)
        {
            IPAddress = ipaddress;
        }

        public override string ToString()
        {
            return IPAddress;
        }

    }
}
