﻿using ExpressVPNClientModel.LocationServer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace ExpressVPNTestLib
{
    class WebApiTests
    {
        private const string MOCKDATAURL = "https://private-16d939-codingchallenge2020.apiary-mock.com/locations";

        private IWebRequestProcessor WebApiProcessor;
        [SetUp]
        public void SetUp()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IWebRequestProcessor, XMLWebRequestProcessor>();

            //These unit tests test the XMLWebRequestProcessor in isolation and do not require the server model
            //
            WebApiProcessor = ServiceLocator.Current.GetInstance<IWebRequestProcessor>();
        }

        [Test]
        public void ValidateParsing()
        {
            Assert.IsTrue(WebApiProcessor != null);

            WebApiProcessor.Process(MOCKDATAURL);

            Assert.True(WebApiProcessor.ResponseXml != null);
            Assert.True(WebApiProcessor.RequestException == null);

            var doc = WebApiProcessor.ResponseXml;

            XmlNodeList iconList = doc.SelectNodes("//expressvpn/icons/icon");
            Assert.True(iconList != null && iconList.Count == 3);


            XmlNodeList locationList = doc.SelectNodes("//expressvpn/locations/location");
            Assert.True(locationList != null && locationList.Count == 6);

            //Expecting Los Angeles as first in list <location name="Los Angeles" sort_order="80" icon_id="5">


            var node = locationList[0];

            Assert.True(node.Attributes["name"].Value == "Los Angeles");

            Assert.True(80 == Convert.ToInt32(node.Attributes["sort_order"].Value));

            Assert.True(5 == Convert.ToInt32(node.Attributes["icon_id"].Value));

        }

        [Test]
        public void TestWebApiForbidden()
        {
            //Test for  403 Forbidden  (mock response)

            var requestFactory = new MockApiWebRequestFactory(new MockWebRequestWithKnownStatusCode(HttpStatusCode.Forbidden));

            WebApiProcessor.Process("http://example.com", requestFactory);

            Assert.IsTrue(WebApiProcessor.ResponseXml == null);
            Assert.IsTrue(WebApiProcessor.RequestException != null);
            Assert.IsTrue(WebApiProcessor.RequestException.Message == XMLWebRequestProcessor.ForbiddenExceptionStr);
        }

        [Test]
        public void TestWebApiBadRequest()
        {
            //Test for 400 Bad Request (mock response)

            var requestFactory = new MockApiWebRequestFactory(new MockWebRequestWithKnownStatusCode(HttpStatusCode.BadRequest));

            WebApiProcessor.Process("http://example.com", requestFactory);

            Assert.IsTrue(WebApiProcessor.ResponseXml == null);
            Assert.IsTrue(WebApiProcessor.RequestException != null);
            Assert.IsTrue(WebApiProcessor.RequestException.Message == XMLWebRequestProcessor.GenericExceptionStr);

        }

        [Test]
        public void TestWebApiTimeoutMock()
        {
            //Test for timeout (mock response)

            var requestFactory = new MockApiWebRequestFactory(new MockWebRequestWithTimeout());
            WebApiProcessor.Process("http://example.com", requestFactory);

            Assert.IsTrue(WebApiProcessor.ResponseXml == null);
            Assert.IsTrue(WebApiProcessor.RequestException != null);
            Assert.IsTrue(WebApiProcessor.RequestException.Message == XMLWebRequestProcessor.TimeoutExceptionStr);

        }


        [Test]
        public void TestWebApiTimeoutReal()
        {
            //Test for timeout - note this makes a real web request

            WebApiProcessor.Process("https://www.google.com:81/");

            Assert.IsTrue(WebApiProcessor.ResponseXml == null);
            Assert.IsTrue(WebApiProcessor.RequestException != null);
            Assert.IsTrue(WebApiProcessor.RequestException.Message == XMLWebRequestProcessor.TimeoutExceptionStr);

        }

        [Test]
        public void TestWebApiBadURL()
        {
            //Test for bad URL - note this makes a real web request
            WebApiProcessor.Process("http://exxxxample.xyzz/serverlocations");

            Assert.IsTrue(WebApiProcessor.RequestException != null);
            Assert.IsTrue(WebApiProcessor.RequestException.Message == XMLWebRequestProcessor.GenericExceptionStr);
        }


    }
}

