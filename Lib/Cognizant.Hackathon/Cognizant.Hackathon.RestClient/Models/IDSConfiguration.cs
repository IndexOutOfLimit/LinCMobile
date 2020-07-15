using Cognizant.Hackathon.RestClient.Interfaces;

namespace Cognizant.Hackathon.RestClient.Models
{
    public class IDSConfiguration : IIDSConfiguration
    {
        public IDSConfiguration(string identityBaseUrl, 
            string identityClientId,
            string identityTenantId,
            string identityClientSecret,
            string identityClientResource,
            string identityScope
            )
        {
            IdentityBaseUrl = identityBaseUrl;
            IdentityClientId = identityClientId;
            IdentityTenantId = identityTenantId;
            IdentityClientSecret = identityClientSecret;
            IdentityClientResource = identityClientResource;
            IdentityScope = identityScope;
        }

        public string IdentityBaseUrl { get; }
        public string IdentityClientId { get; }
        public string IdentityTenantId { get; }
        public string IdentityClientSecret { get; }
        public string IdentityClientResource { get; }
        public string IdentityScope { get; }
    }
}