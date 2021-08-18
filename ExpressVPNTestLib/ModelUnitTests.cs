using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ExpressVPNClientModel;

namespace ExpressVPNTestLib
{
    class ModelUnitTests
    {
        private const string AIX = "AIX-EN-PROVENCE";
        private const string BERLIN = "BERLIN";

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestLocationManager()
        {
            Assert.NotNull(ServerModel.Instance.LocationMgr);

            Assert.True(ServerModel.Instance.LocationMgr.ToList().Count == 0);

            ServerModel.Instance.LocationMgr.AddUpdate(AIX, 85, 0);
            ServerModel.Instance.LocationMgr.AddUpdate(BERLIN, 21, 0);

            Assert.True(ServerModel.Instance.LocationMgr.ToList().Count == 2);

            //Neither location has IP address
            Assert.True(ServerModel.Instance.LocationMgr.PresentationList().Count == 0);

            var aix = ServerModel.Instance.LocationMgr.Lookup(AIX);
            Assert.IsTrue(AIX != null);
            aix.AddAddress("113.77.335.0");
            Assert.True(ServerModel.Instance.LocationMgr.PresentationList().Count == 1);

            var berlin = ServerModel.Instance.LocationMgr.Lookup(BERLIN);
            Assert.IsTrue(berlin != null);
            berlin.AddAddress("228.92.34.11");

            //Now both have at least one IP address
            Assert.True(ServerModel.Instance.LocationMgr.PresentationList().Count == 2);

            //Test sort order

            //Berlin (21)
            Assert.True(ServerModel.Instance.LocationMgr.PresentationList().FirstOrDefault() == berlin);

            //Aix (85)
            Assert.True(ServerModel.Instance.LocationMgr.PresentationList().LastOrDefault() == aix);

        }


    }
}
