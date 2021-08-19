using NUnit.Framework;
using PingService;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using PingService;

namespace ExpressVPNTestLib
{
    public class PingServiceTests : IAddressProvider
    {

        private CancellationTokenSource CTSource;
        private readonly List<IPAddress> IPList = new List<IPAddress>();


        public List<IPAddress> GetAddressList()
        {
            return IPList;
        }

        [SetUp]
        public void Setup()
        {
            IPList.Add(new IPAddress("151.101.192.81")); //bbc.com
            IPList.Add(new IPAddress("113.77.335.0")); //invalid host 

            CTSource = new CancellationTokenSource();

            PingService.PingService.Start(this, CTSource.Token, null, 1000);

        }

        [Test]
        public async Task TestPingHosts()
        {
            //Allow 2 seconds for pings to complete
            await Task.Delay(2000);

            Assert.IsTrue(IPList.First().PingRoundTrip.HasValue);

            Assert.IsTrue(!IPList.Last().PingRoundTrip.HasValue);
        }

        [Test]
        public void TestCloseService()
        {
            CTSource.Cancel();


            Assert.IsTrue( PingService.PingService.SericeStopped.WaitOne(5000));

        }



    }
}