using Cognizant.Hackathon.RestClient.Interfaces;

namespace Cognizant.Hackathon.RestClient.Models
{
    public class ApiConfiguration : IApiConfiguration
    {
        public ApiConfiguration(string baseUrl, string authUrl, string localBaseUrl, string contentBaseUrl, double apiTimeout, bool enableUrlRewrite)
        {
            BaseUrl = baseUrl;
            AuthUrl = authUrl;
            LocalBaseUrl = localBaseUrl;
            ContentBaseUrl = contentBaseUrl;
            ApiTimeout = apiTimeout;
            EnableUrlRewrite = enableUrlRewrite;
        }

        public string BaseUrl { get; }
        public string AuthUrl { get; }
        public string LocalBaseUrl { get; }
        public string ContentBaseUrl { get; }
        public double ApiTimeout { get; }
        public bool EnableUrlRewrite { get; }
    }
}
