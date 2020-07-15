using Cognizant.Hackathon.RestClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Cognizant.Hackathon.RestClient.Providers
{
    public abstract class UrlRewriteProviderBase : IUrlRewriteProvider
    {
        protected readonly IApiConfiguration _apiConfiguration;

        public Dictionary<string, string> UrlMapping { get; set; }

        protected UrlRewriteProviderBase(IApiConfiguration apiConfiguration)
        {
            _apiConfiguration = apiConfiguration;

            if (apiConfiguration.EnableUrlRewrite)
                SetUrlMapping();
        }

        protected abstract void SetUrlMapping();

        public HttpRequestMessage Rewrite(HttpRequestMessage message)
        {
            if (!_apiConfiguration.EnableUrlRewrite || UrlMapping == null)
                return message;

            foreach (var mapping in UrlMapping)
            {
                if (!message.RequestUri.AbsoluteUri.ToLower().Contains(mapping.Key.ToLower()))
                    continue;

                var path = message.RequestUri.PathAndQuery.ToLower().Replace(mapping.Key.ToLower(), "");
                path = path.Replace("/api", "");
                var uri = mapping.Value + path;

                message.RequestUri = new Uri(uri);
                return message;
            }

            return message;
        }

        public string Rewrite(string url)
        {
            if (!_apiConfiguration.EnableUrlRewrite || UrlMapping == null)
                return url;

            foreach (var mapping in UrlMapping)
            {
                if (!url.ToLower().Contains(mapping.Key.ToLower()))
                    continue;

                var index = url.ToLower().IndexOf(mapping.Key.ToLower());
                var result = mapping.Value.ToLower() + url.Substring(index + mapping.Key.Length);
                return result;
            }

            return url;
        }


    }
}
