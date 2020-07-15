using System;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Providers;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Models
{
    public class RestConfiguration : IRestConfiguration
    {
        public IUrlRewriteProvider UrlRewriteProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IApiConfiguration ApiConfiguration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double TimeOut => throw new NotImplementedException();
    }
}
