using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;

namespace PingService
{
    public class PingService
    { 

        public const int MinimumPollingInterval = 1000;
        public static AutoResetEvent SericeStopped {get; private set; } = new AutoResetEvent(false);

        public static void Start(IAddressProvider provider, CancellationToken token, Action uiRfresh, int pollingInterval = 8000)
        {
            if (provider == null)
                throw new ArgumentNullException("IAddressProvider");

            pollingInterval = Math.Max(MinimumPollingInterval, pollingInterval);

            _ = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await DelayObservingCancellation(token, pollingInterval);

                    foreach (IPAddress a in provider.GetAddressList())
                    {
                        if (a.Ping())
                            uiRfresh?.Invoke();

                        if (token.IsCancellationRequested)
                            break;
                    }
                }

                _ = SericeStopped.Set();

            });

        }

        private static async Task DelayObservingCancellation(CancellationToken token, int delaymilliseconds)
        {
            if (delaymilliseconds <= 0)
                return;

            long totalDelay = 0;

            while ((totalDelay < delaymilliseconds) && !token.IsCancellationRequested)
            {
                await Task.Delay(500);
                totalDelay += 500;
            }
        }


        public static long PingHost(string nameOrAddress)
        {
            try
            {
                using (var pinger = new Ping())
                {
                    PingReply reply = pinger.Send(nameOrAddress);

                    return reply.Status == IPStatus.Success ? reply.RoundtripTime : -1;
                }
            }
            catch (Exception ex)
            {
                int brk = 1;
            }

            return -1;




        }
    }
}
