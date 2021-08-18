using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel
{
    public class ServerIcon
    {

        public int Id { get; private set; }
        public string Base64Encoding { get; private set; }

        internal ServerIcon(int id, string base64encoding)
        {
            Id = id;
            Base64Encoding = base64encoding;
        }

    }
}
