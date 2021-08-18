using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel
{
    public class IconStore
    {

        private Dictionary<int, ServerIcon> Collection = new Dictionary<int, ServerIcon>();

        //private static IconStore Instance = Icons;

        /*
        public static IconStore Icons
        {
            get
            {
                if (Instance == null)
                    Instance = new IconStore();
                return Instance;
            }
        }*/

        public void Add(int id, string base64Encoding)
        {
            Collection[id] = new ServerIcon(id, base64Encoding);
        }

        public ServerIcon Lookup(int id)
        {
            if (Collection.ContainsKey(id))
                return Collection[id];

            return null;

        }


    }
}
