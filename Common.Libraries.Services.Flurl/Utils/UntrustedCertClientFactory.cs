using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Common.Libraries.Services.Flurl.Utils
{
    public class UntrustedCertClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, b, c, d) => true
            };
        }
    }
}
