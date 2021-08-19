using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace ExpressVPNClientModel
{
    public class PingService
    {

        public static AutoResetEvent SericeStopped = new AutoResetEvent(false);

        public static void Start(LocationManager locMgr, CancellationToken token, int pollingInterval = 10000)
        {
            if (locMgr == null)
                throw new ArgumentException("LocationManager");

            _ = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await DelayObservingCancellation(token, pollingInterval);

                    if (token.IsCancellationRequested)
                        break;

                    foreach (ServerLocation sl in locMgr.PresentationList())
                        sl.PingAddresses();
                }

                _ = SericeStopped.Set();

            });

        }

        private static async Task DelayObservingCancellation(CancellationToken token, int delaymilliseconds)
        {
            long totalDelay = 0;

            while ((totalDelay < delaymilliseconds) && !token.IsCancellationRequested)
            {
                await Task.Delay(500);
                totalDelay += 500;
            }

        }


        public static long PingHost(string nameOrAddress)
        {

            using (var pinger = new Ping())
            {
                PingReply reply = pinger.Send(nameOrAddress);

                return reply.Status == IPStatus.Success ? reply.RoundtripTime : -1;
            }


        }
    }


}
