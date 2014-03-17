using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Axed
{
    public class SSOClient : WebClient
    {
        public CookieContainer CookieContainer { get; private set; }

        public SSOClient()
        {
            CookieContainer = new CookieContainer();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = CookieContainer;
            }
            return request;
        }
    }
}
