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
        private const int TESTSERVERCOUNT = 25;

        [SetUp]
        public void Setup()
        {
            ServerModel.Instance.Load192TestServers(TESTSERVERCOUNT);
        }

        [Test]
        public void Test1()
        {

            Assert.True(ServerModel.Instance.LocationMgr.ToList().Count == TESTSERVERCOUNT);
        }
    }
}
