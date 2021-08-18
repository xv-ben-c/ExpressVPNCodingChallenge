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

            ServerList.PrimaryServerList.Load192TestServers(TESTSERVERCOUNT);
        }

        [Test]
        public void Test1()
        {

            Assert.True(ServerList.PrimaryServerList.Servers.Count == TESTSERVERCOUNT);
        }
    }
}
