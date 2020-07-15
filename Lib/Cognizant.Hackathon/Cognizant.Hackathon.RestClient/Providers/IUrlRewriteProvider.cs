using System.Collections.Generic;
using System.Net.Http;

namespace Cognizant.Hackathon.RestClient.Providers
{    
    public interface IUrlRewriteProvider
    {       
        HttpRequestMessage Rewrite(HttpRequestMessage message);     
        string Rewrite(string url);       
        Dictionary<string, string> UrlMapping { get; set; }
    }
}