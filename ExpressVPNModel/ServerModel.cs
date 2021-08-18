using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExpressVPNClientModel
{
    public class ServerModel
    {

        public LocationManager LocationMgr {get; private set; }  = new LocationManager();

        public IconStore Icons { get; private set; } = new IconStore();

        private static ServerModel _Instance;
        public static ServerModel Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ServerModel();

                return _Instance;
            }
        }

        private ServerModel()
        {

        }

        public void Load192TestServers(int count)
        {
            LocationMgr.AddUpdate("localhost", 0, 0);

            var lh = LocationMgr.Lookup("localhost");

            for (int i = 1; i <= count; i++)
            {
                lh.AddAddress($"192.168.0.{i}");
            }
        }

        public async Task RefreshAsync(string url)
        {

            var rp = new XMLWebRequestProcessor(url);

            if (rp.RequestException != null)
            {
                //FIXME
            }
            else
            {

                //New or existing servers in the response will be set to Available
                Update(rp.Response);
            }
        }

        private void Update(XmlDocument response)
        {
            if (response != null)
            {
                XmlNodeList iconList = response.SelectNodes("//expressvpn/icons/icon");

                var locationList = response.SelectNodes("//expressvpn/locations/location");

                foreach (XmlNode icon in iconList)
                {
                    Icons.Add(Convert.ToInt32(icon.Attributes["id"].Value), icon.InnerXml.Trim());
                }


                foreach (XmlNode locn in locationList)
                {
                    string name = locn.Attributes["name"].Value;
                    int sortOrder = Convert.ToInt32(locn.Attributes["sort_order"].Value);
                    int icon_id = Convert.ToInt32(locn.Attributes["icon_id"].Value);

                    ServerLocation sl = LocationMgr.AddUpdate(name, sortOrder, icon_id);
                    sl.ClearAddressList();

                    foreach (XmlNode svr in locn.ChildNodes)
                    {
                        string ip = svr.Attributes["ip"].Value;
                        sl.AddAddress(ip);
                    }
                }
            }
        }
    }
}
