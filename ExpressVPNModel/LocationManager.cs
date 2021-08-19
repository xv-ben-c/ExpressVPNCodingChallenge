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
            return Locations.Values.Where(x => x.Available).OrderBy(x => x.SortOrder).ToList();
        }

        public int AvailableLocations
        {
            get { return Locations.Values.Where(x => x.Available).Count(); }
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

        /// <summary>
        /// Run query over ServerLocation: return the sl which has an IP with overall lowest ping RT time
        /// /// </summary>
        /// <returns></returns>
        public ServerLocation BestServerLocation()
        {
            var sl = Locations.Values.Where(x => x.Available).OrderBy(x => x.MinRoundTripAddress).FirstOrDefault();

            if (sl==null)
                return null;

            return sl.MinRoundTripAddress < int.MaxValue ? sl: null;
        }

        public bool PingComplete()
        {
            foreach(var sl in Locations.Values)
            {
                if (!sl.PingComplete)
                    return false;
            }
            return true;
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
