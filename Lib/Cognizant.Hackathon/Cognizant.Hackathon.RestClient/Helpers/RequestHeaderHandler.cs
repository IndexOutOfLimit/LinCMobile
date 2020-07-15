using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    public class RequestHeaderHandler : DelegatingHandler
    {
        private readonly Dictionary<string, string> _headers;

        public RequestHeaderHandler(Dictionary<string, string> headers)
        {
            _headers = headers;
            InnerHandler = new HttpClientHandler();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_headers != null)
            {
                foreach (var header in _headers)
                {
                    request.Headers.Remove(header.Key);
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
