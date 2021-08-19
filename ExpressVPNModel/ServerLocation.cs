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
        private List<PingService.IPAddress> AddressesList = new List<PingService.IPAddress>();

        public string Location { get; private set; }

        public int SortOrder { get; private set; }

        public int IconId { get; private set; }

        private IIconProvider IconProvider;

        internal ServerLocation(string location, int sortOrder, int iconId, IIconProvider iconstore)
        {
            Location = location;
            SortOrder = sortOrder;
            IconId = iconId;
            IconProvider = iconstore;
        }


        /// <summary>
        /// A ServerLocation is deemed Available is there exists >0 assoicated IP addresses
        /// </summary>
        public bool Available
        {
            get { return AddressesList.Where(a => !a.Offline).Any(); }
        }

        public ImageSource Icon
        {
            get
            {
                return IconProvider!=null ? IconProvider.GetIcon(IconId) : null;
            }
        }

        internal void AppendOnlineAddresses(List<PingService.IPAddress> lst)
        {
            if (lst != null)
                lst.AddRange(AddressesList.Where(a => !a.Offline).ToList());
        }

        public string LocationStatus
        {
            get
            {
                if (!Available)
                    return "-";

                long? minrt = MinRoundTripAddress;

                return minrt == null ? "..." : $"{minrt}ms";
            }
        }

        internal void PingAddresses()
        {
            AddressesList.ForEach(a => a.Ping());
        }

        /// <summary>
        /// Returns the minimum of the ping round trip times for all IPs associated with this location
        /// </summary>
        internal long? MinRoundTripAddress
        {
            get
            {
                long minrt = int.MaxValue;

                foreach (var a in AddressesList)
                {
                    if (!a.Offline && a.PingRoundTrip.HasValue)
                        minrt = Math.Min(minrt, a.PingRoundTrip.Value);
                }

                return minrt == int.MaxValue ? null : (long?)minrt;
            }
        }



        internal void Update(int sortOrder, int iconId)
        {
            SortOrder = sortOrder;
            IconId = iconId;
        }

        internal void SetAllOffline()
        {
            AddressesList.ForEach(a => a.SetOffline(true));
        }

        public void AddAddress(string addr)
        {
            if (!string.IsNullOrEmpty(addr))
            {

                var ip = AddressesList.FirstOrDefault(x => x.Address == addr);

                if (ip == null)
                {
                    AddressesList.Add(new PingService.IPAddress(addr));
                }
                else
                {
                    ip.SetOffline(false);
                }
            }

        }

    }
}
