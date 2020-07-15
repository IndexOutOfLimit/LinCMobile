using Cognizant.Hackathon.RestClient.Providers;

namespace Cognizant.Hackathon.RestClient.Interfaces
{
    public interface IRestConfiguration
    {
        // Others setup properties can be added here
        IUrlRewriteProvider UrlRewriteProvider { get; set; }

        IApiConfiguration ApiConfiguration { get; set; }

        double TimeOut { get; }
    }
}
