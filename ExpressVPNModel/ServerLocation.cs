using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ExpressVPNClientModel
{
    public class ServerLocation
    {

        public string Location { get; private set; }

        public int SortOrder { get; private set; }

        public int IconId { get; private set; }


        /// <summary>
        /// A ServerLocation is deemed Available is there exists >0 assoicated IP addresses
        /// </summary>
        public bool Available
        {
            get { return AddressesList.Count > 0; }
        }

        public ImageSource Icon
        {
            get
            {
                ServerIcon si = ServerModel.Instance.Icons.Lookup(IconId);

                return si?.IconImageSource;
            }
        }

        internal void PingAddresses()
        {
            AddressesList.ForEach(a => a.Ping());
        }

        /// <summary>
        /// Returns the minimum of the ping round trip times for all IPs associated with this location
        /// </summary>
        internal long MinRoundTripAddress
        {
            get
            {
                long minrt = int.MaxValue;

                foreach (var a in AddressesList)
                {
                    if (a.PingRoundTrip.HasValue)
                        minrt = Math.Min(minrt, a.PingRoundTrip.Value);
                }
                return minrt;
            }
        }

        public List<IPAddress> AddressesList { get; private set; } = new List<IPAddress>();
        public bool PingComplete
        {
            get
            {
                foreach (var a in AddressesList)
                {
                    if (!a.PingRoundTrip.HasValue)
                       return false;
                }
                return true;
            }
        }

        internal ServerLocation(string location, int sortOrder, int iconId)
        {
            Location = location;
            SortOrder = sortOrder;
            IconId = iconId;
        }

        internal void Update(int sortOrder, int iconId)
        {
            SortOrder = sortOrder;
            IconId = iconId;
        }

        internal void ClearAddressList()
        {
            AddressesList.Clear();
        }

        public void AddAddress(string addr)
        {
            if (!string.IsNullOrEmpty(addr))
            {
                //Ignore duplicates

                var ip = AddressesList.FirstOrDefault(x => x.Address == addr);
                if (ip == null)
                {
                    AddressesList.Add(new IPAddress(addr));
                }
            }

        }

    }
}
