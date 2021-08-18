using ExpressVPNClientModel.LocationServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExpressVPNTestLib
{
    public class MockApiWebRequestFactory : WebRequestFactory
    {

        private IHttpWebRequest PrebuiltRequest;

        public MockApiWebRequestFactory(IHttpWebRequest req = null)
        {
            PrebuiltRequest = req;
        }

        public override IHttpWebRequest Create(string url, string method, string contentType)
        {
            return PrebuiltRequest!=null ? PrebuiltRequest : base.Create(url, method, contentType);
        }

    }

    public class MockWebRequestWithKnownStatusCode : IHttpWebRequest
    {
        private HttpStatusCode ReturnCode;
        public MockWebRequestWithKnownStatusCode(HttpStatusCode scode)
        {
            ReturnCode = scode;
        }

        public string Method { get => "GET"; set => throw new NotImplementedException(); }
        public string ContentType { get => "xml"; set => throw new NotImplementedException(); }

        public IHttpWebResponse GetResponse()
        {
            return new MockWebResponse(ReturnCode);
        }
    }

    public class MockWebRequestWithTimeout : IHttpWebRequest
    {
        public string Method { get => "GET"; set => throw new NotImplementedException(); }
        public string ContentType {get => "xml";  set => throw new NotImplementedException(); }

        public IHttpWebResponse GetResponse()
        {
            throw new  WebException("Timeout", WebExceptionStatus.Timeout);
        }
    }

    class MockWebResponse : IHttpWebResponse
    {
        public MockWebResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public HttpStatusCode StatusCode { get; set; }

        public Stream GetResponseStream()
        {
            throw new NotImplementedException();
        }
    }
}
