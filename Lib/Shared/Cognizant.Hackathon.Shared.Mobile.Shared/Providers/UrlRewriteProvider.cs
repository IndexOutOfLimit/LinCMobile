using System;
using System.Collections.Generic;
using System.Text;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Providers;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Providers
{
    public class UrlRewriteProvider: UrlRewriteProviderBase
    {
        public UrlRewriteProvider(IApiConfiguration apiConfiguration) : base(apiConfiguration)
        {
        }

        protected override void SetUrlMapping()
        {
           
        }
    }
}
