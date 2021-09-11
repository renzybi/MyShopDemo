using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private HttpCookieCollection cookies;
        private MockRequest request;
        private MockRespose response;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.response = new MockRespose(cookies);
        }
        public override HttpRequestBase Request
        {
            get { return request; }
        }
        public override HttpResponseBase Response
        {
            get { return response; }
        }
    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;

        public MockRequest(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }
    }

    public class MockRespose : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies;

        public MockRespose(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }
    }
}
