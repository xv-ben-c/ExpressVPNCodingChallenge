using CommonServiceLocator;
using ExpressVPNClientModel.LocationServer;
using PingService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace ExpressVPNClientModel
{


    public interface IIconProvider
    {
        ImageSource GetIcon(int id);
    }


    public class ServerModel : IAddressProvider, IIconProvider
    {
        private string ServerLocatorURI;

        private IServiceLocator ServiceLocator;

        private object ModelLock = new object();

        public LocationManager LocationMgr { get; private set; } = new LocationManager();

        public IconStore Icons { get; private set; } = new IconStore();

        private readonly CancellationTokenSource CTSource = new CancellationTokenSource();

        #region Statics

        private static ServerModel _Instance;

        public static void Init(IServiceLocator serviceLocator, string serverlocationsUri, Action uiDel = null, bool startPingService = true)
        {
            if (_Instance == null)
            {
                _Instance = new ServerModel(serviceLocator, serverlocationsUri, uiDel, startPingService);
            }
        }
        public static ServerModel Instance
        {
            get { return _Instance; }
        }
        #endregion

        private ServerModel(IServiceLocator serviceLocator, string serverlocatorURL, Action uiDel, bool startPingService)
        {
            if (serviceLocator == null)
                throw new ArgumentNullException("DI Service Locator");

            if (string.IsNullOrEmpty(serverlocatorURL))
                throw new ArgumentNullException("Server Locator URL");

            ServerLocatorURI = serverlocatorURL;

            ServiceLocator = serviceLocator;

            if (startPingService)
                PingService.PingService.Start(this, CTSource.Token, uiDel);
        }

        public void Shutdown()
        {
            CTSource.Cancel();
            PingService.PingService.SericeStopped.WaitOne();
            Debug.WriteLine("Ping Service Stopped...");
            _Instance = null;
        }

        public string RefreshButtonText { get; private set; } = "Refresh";

        public async Task RefreshAsync()
        {
            lock (ModelLock)
            {
                try
                {
                    IRequestProcessor wprc = ServiceLocator.GetInstance<IRequestProcessor>();

                    var doc = wprc.Process(ServerLocatorURI);

                    if (wprc.RequestException != null)
                        throw wprc.RequestException;

                    Update(doc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ExpressVPN Client", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

                LocationMgr.SetAllOffline();

                foreach (XmlNode locn in locationList)
                {
                    string name = locn.Attributes["name"].Value;
                    int sortOrder = Convert.ToInt32(locn.Attributes["sort_order"].Value);
                    int icon_id = Convert.ToInt32(locn.Attributes["icon_id"].Value);

                    ServerLocation sl = LocationMgr.AddUpdate(name, sortOrder, icon_id);
                    Debug.Assert(sl != null);
                    

                    foreach (XmlNode svr in locn.ChildNodes)
                    {
                        string ip = svr.Attributes["ip"].Value;
                        sl.AddAddress(ip); //Sets the ipaddress "online"
                    }
                }

                var textNode = responseDoc.SelectSingleNode("//expressvpn/button_text");
                if (textNode != null)
                {
                    RefreshButtonText = textNode.InnerText;
                }


            }
        }


        #region Interface IAddressProvider
        public List<IPAddress> GetAddressList()
        {
            lock (ModelLock)
            {
                return LocationMgr.OnlineAddressList();
            }
        }
        #endregion

        #region IIconStore
        public ImageSource GetIcon(int id)
        {
            ServerIcon si = Icons.Lookup(id);
            return si?.IconImageSource;
        }
        #endregion

    }
}
