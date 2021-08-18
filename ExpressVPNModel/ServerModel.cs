using ExpressVPNClientModel.LocationServer;
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

        #region Statics

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
        #endregion

        private ServerModel()
        {

        }

        

        public string RefreshButtonText { get;private set;} = "Refresh";

        public async Task RefreshAsync(string url)
        {
            var rp = new XMLWebRequestProcessor(url);

            if (rp.RequestException != null)
            {
                //FIXME
            }
            else
            {
                Update(rp.ResponseXml);
            }
        }

        /// <summary>
        /// Create or update LocationServer collection
        /// Create or update Icons collection
        /// Set button text
        /// </summary>
        /// <param name="responseDoc"></param>
        private void Update(XmlDocument responseDoc)
        {
            if (responseDoc != null)
            {
                XmlNodeList iconList = responseDoc.SelectNodes("//expressvpn/icons/icon");

                var locationList = responseDoc.SelectNodes("//expressvpn/locations/location");

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

                var textNode = responseDoc.SelectSingleNode("//expressvpn/button_text");
                if (textNode!=null)
                {
                    RefreshButtonText = textNode.InnerText;
                }


            }
        }
    }
}
