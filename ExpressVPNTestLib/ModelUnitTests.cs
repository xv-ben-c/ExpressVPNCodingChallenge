﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ExpressVPNClientModel;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using ExpressVPNClientModel.LocationServer;

namespace ExpressVPNTestLib
{
    class ModelUnitTests
    {
        private const string AIX = "AIX-EN-PROVENCE";
        private const string BERLIN = "BERLIN";

        [SetUp]
        public void Setup()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IRequestProcessor, XMLWebRequestProcessor>();

            //Note - PingService not started here. Tested separetely (PingServiceTests.cs)
            ServerModel.Init(ServiceLocator.Current, "https://private-16d939-codingchallenge2020.apiary-mock.com/locations", null, false);

            Assert.NotNull(ServerModel.Instance);
            Assert.NotNull(ServerModel.Instance.LocationMgr);
        }

        [Test]
        public void TestLocationManager()
        {

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

        [Test]
        public async Task TestMockData()
        {
            Assert.True(ServerModel.Instance.LocationMgr.ToList().Count == 2);

             //Load an additional 6 locations from mock api
            await ServerModel.Instance.RefreshAsync();

            Assert.True(ServerModel.Instance.LocationMgr.PresentationList().Count == 8);

            //Expected Sort order
            /*
              <location name="Berlin" sort_order="21" >
              <location name="Los Angeles" sort_order="80">
              <location name="AIX-EN-PROVENCE" sort_order="85">
              <location name="LA 2 - Best for North China" sort_order="90">
              <location name="New York City" sort_order="100" ">
              <location name="UK - Berkshire" sort_order="175">
              <location name="UK - London" sort_order="185" >
              <location name="UK - Isle of Man" sort_order="195" >
             
             */

            Assert.IsTrue(ServerModel.Instance.LocationMgr.PresentationList()[0].Location == BERLIN);
            Assert.IsTrue(ServerModel.Instance.LocationMgr.PresentationList()[1].Location == "Los Angeles");
            Assert.IsTrue(ServerModel.Instance.LocationMgr.PresentationList()[2].Location == AIX);


        }


    }
}
