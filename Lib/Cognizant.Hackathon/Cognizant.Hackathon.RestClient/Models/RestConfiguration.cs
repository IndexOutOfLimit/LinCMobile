using System;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Providers;

namespace Cognizant.Hackathon.RestClient.Models
{
    public class RestConfiguration : IRestConfiguration
    {
        const double DEFAULT_TIMEOUT = 30000;

        public RestConfiguration(IUrlRewriteProvider urlRewriteProvider, IApiConfiguration apiConfiguration)
        {
            UrlRewriteProvider = urlRewriteProvider;
            ApiConfiguration = apiConfiguration;

            if (apiConfiguration.ApiTimeout <= 0) TimeOut = DEFAULT_TIMEOUT;
            else TimeOut = apiConfiguration.ApiTimeout;

            if (UrlRewriteProvider == null)
                throw new ArgumentNullException("UrlRewriteProvider should not be null");

            if (ApiConfiguration == null)
                throw new ArgumentNullException("ApiConfiguration should not be null");
        }

        /// <summary>
        /// Gets or sets the URL rewrite provider.
        /// </summary>
        public IUrlRewriteProvider UrlRewriteProvider { get; set; }

        /// <summary>
        /// Gets or sets the API configuration.
        /// </summary>
        public IApiConfiguration ApiConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the time out.
        /// </summary>
        public double TimeOut { get; }
    }
}