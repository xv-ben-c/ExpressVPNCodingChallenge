using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNClientModel
{
    public class LocationManager
    {

        private Dictionary<string, ServerLocation> Locations = new Dictionary<string, ServerLocation>();


        internal LocationManager()
        {

        }

        public List<ServerLocation> ToList()
        {
            return Locations.Values.ToList();
        }

        public List<ServerLocation> PresentationList()
        {
             return Locations.Values.Where(x=>x.Available).OrderBy(x=>x.SortOrder).ToList();
        }

        public ServerLocation AddUpdate(string locn, int sortOrder, int iconId)
        {
            ServerLocation sl = Lookup(locn);

            if (sl == null)
            {
                sl = new ServerLocation(locn, sortOrder, iconId);
                Locations.Add(locn.ToLower(), sl);
            }
            else
            {
                sl.Update(sortOrder, iconId);
            }
            return sl;
        }


        public ServerLocation Lookup(string locn)
        {
            return Locations.ContainsKey(locn.ToLower()) ? Locations[locn.ToLower()] : null;
        }

        internal void DeleteAddresses()
        {
            foreach (var sl in Locations.Values)
            {
                sl.ClearAddressList();
            }
        }
    }
}
