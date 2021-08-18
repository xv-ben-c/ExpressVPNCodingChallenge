using ExpressVPNClientModel.LocationServer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace ExpressVPNTestLib
{
    class WebApiTests
    {
        private const string MOCKDATAURL = "https://private-16d939-codingchallenge2020.apiary-mock.com/locations";




        [SetUp]
        public void SetUp()
        {

        }

        [Test]

        public void ValidateParsing()
        {
            var webApi = new XMLWebRequestProcessor(MOCKDATAURL);

            Assert.True(webApi.ResponseXml != null);
            Assert.True(webApi.RequestException == null);

            var doc = webApi.ResponseXml;

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
        public void TestWebApiErrorHandling()
        {
            //Test for  403 Forbidden  (mock response)
            {
                var webApi = new XMLWebRequestProcessor("http://example.com", new MockApiWebRequestFactory(
                     new MockWebRequestWithKnownStatusCode(HttpStatusCode.Forbidden)));

                Assert.IsTrue(webApi.RequestException != null);
                Assert.IsTrue(webApi.RequestException.Message == XMLWebRequestProcessor.ForbiddenExceptionStr);
            }


            //Test for 400 Bad Request (mock response)
            {
                var webApi = new XMLWebRequestProcessor("http://example.com", new MockApiWebRequestFactory(
                     new MockWebRequestWithKnownStatusCode(HttpStatusCode.BadRequest)));

                Assert.IsTrue(webApi.RequestException != null);
                Assert.IsTrue(webApi.RequestException.Message == XMLWebRequestProcessor.GenericExceptionStr);
            }

            //Test for timeout (mock response)
            {
                var webApi = new XMLWebRequestProcessor("http://example.com", new MockApiWebRequestFactory(
                    new MockWebRequestWithTimeout()));

                Assert.IsTrue(webApi.RequestException != null);
                Assert.IsTrue(webApi.RequestException.Message == XMLWebRequestProcessor.TimeoutExceptionStr);
            }

            //Test for timeout - note this makes a real web request
            {
                var webApi = new XMLWebRequestProcessor("https://www.google.com:81/");
                Assert.IsTrue(webApi.RequestException != null);
                Assert.IsTrue(webApi.RequestException.Message == XMLWebRequestProcessor.TimeoutExceptionStr);
            }


            //Test for bad URL - note this makes a real web request
            {
                var webApiMock = new XMLWebRequestProcessor("http://exxxxample.xyzz/serverlocations");

                Assert.IsTrue(webApiMock.RequestException != null);
                Assert.IsTrue(webApiMock.RequestException.Message == XMLWebRequestProcessor.GenericExceptionStr);
            }





        }
    }
}
