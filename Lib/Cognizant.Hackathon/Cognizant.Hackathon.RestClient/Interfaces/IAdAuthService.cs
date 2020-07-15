using System;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.RestClient.Interfaces
{
    public interface IAdAuthService
    {
        Task<ServiceResponse<(AuthToken authToken, Guid UniqueId, string Username)>> Authenticate(object parent);
        
        Task<ServiceResponse<bool>> Unauthenticate();

        Task<AuthToken> GetTokenFromCache();
    }
}
