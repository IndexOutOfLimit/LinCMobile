namespace Cognizant.Hackathon.RestClient.Interfaces
{
    public interface IIDSConfiguration
    {
       
        // IdentityServer Authority Url
        string IdentityBaseUrl { get; }

        // IdentityServer USER API Authentication
        string IdentityClientId { get; }
        string IdentityTenantId { get; }
        string IdentityClientSecret { get; }
        string IdentityClientResource { get; }
        string IdentityScope { get; }

        
    }
}