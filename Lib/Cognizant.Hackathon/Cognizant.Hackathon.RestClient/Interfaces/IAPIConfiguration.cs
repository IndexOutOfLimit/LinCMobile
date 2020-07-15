namespace Cognizant.Hackathon.RestClient.Interfaces
{
    public interface IApiConfiguration
    {
        string BaseUrl { get; }
        string AuthUrl { get; }
        string LocalBaseUrl { get; }
        string ContentBaseUrl { get; }
        double ApiTimeout { get; }
        bool EnableUrlRewrite { get; }
    }
}