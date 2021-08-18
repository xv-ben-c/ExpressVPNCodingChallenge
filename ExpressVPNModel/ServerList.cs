using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExpressVPNClientModel
{
    public class ServerList
    {

        private static ServerList Instance = PrimaryServerList;

        public static ServerList PrimaryServerList
        {
            get
            {
                if (Instance == null)
                    Instance = new ServerList();
                return Instance;
            }

        }


        public List<VPNServer> Servers { get; private set; } = new List<VPNServer>();

        public Dictionary<int, ServerIcon> Icons { get; private set; } = new Dictionary<int, ServerIcon>();



        private ServerList()
        {
           
        }

        public void Load192TestServers(int count)
        {
             for (int i = 1; i <= count; i++)
                Servers.Add(new VPNServer($"192.168.0.{i}"));
        }

        public void Refresh()
        {
            XmlDocument locations = LocationAPI.FetchLocations();
            if (locations != null)
            {
                XmlNodeList iconList = locations.SelectNodes("//expressvpn/icons/icon");

                var locationList = locations.SelectNodes("//expressvpn/locations/location");

                foreach (XmlNode icon in iconList)
                {
                    int id = System.Convert.ToInt32(icon.Attributes["id"].Value);
                    var content = icon.InnerXml.Trim();
                    Icons[id] = new ServerIcon(id, content);
                }


                foreach (XmlNode locn in locationList)
                {
                    string name = locn.Attributes["name"].Value;
                    int  sortOrder = System.Convert.ToInt32(locn.Attributes["sort_order"].Value);
                    int  icon_id = System.Convert.ToInt32(locn.Attributes["icon_id"].Value);
                    
                    foreach (XmlNode svr in locn.ChildNodes)
                    {
                        string ip = svr.Attributes["ip"].Value;
                    }

                }


            }


        }

    }
}
