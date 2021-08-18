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

        public bool Available
        {
            get { return AddressesList.Count > 0; }
        }

        public ImageSource Icon
        {
            get
            {
                var si = ServerModel.Instance.Icons.Lookup(IconId);

                return si==null ? null : si.IconImageSource;
            }
        }

        public List<IPAddress> AddressesList { get; private set; } = new List<IPAddress>();


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
