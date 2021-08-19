using System.Diagnostics;

namespace PingService
{
    public class IPAddress
    {
        public string Address { get; private set; }

        public bool Offline { get; private set; }

        public long? PingRoundTrip { get; private set; } = null;

        public IPAddress(string ipaddr)
        {
            Address = ipaddr;
            Offline = false;
        }

        public void SetOffline(bool isOffline)
        {
            Offline = isOffline;
        }

        public bool Ping()
        {
            if (!Offline)
            {
                Debug.WriteLine($"PING {Address}");

                long rt = PingService.PingHost(Address);
                if (rt > 0)
                {
                    PingRoundTrip = rt;
                    Debug.WriteLine($"PING-SUCCESS {Address} == {PingRoundTrip}");
                    return true;
                }
                else
                    Debug.WriteLine($"PING-ERROR {Address}");
            }

            return false;

        }
    }
}
