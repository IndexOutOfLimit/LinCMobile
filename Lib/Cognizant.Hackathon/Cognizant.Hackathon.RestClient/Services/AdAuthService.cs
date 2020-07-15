using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Helpers;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.RestClient.Services
{
    public class AdAuthService : IAdAuthService
    {
        private string[] _scopes = { "User.Read" };


        public AdAuthService(IIDSConfiguration idsConfiguration)
        {            
            
        }

        public async Task<ServiceResponse<(AuthToken authToken, Guid UniqueId, string Username)>> Authenticate(object parent)
        {
            return new ServiceResponse<(AuthToken authToken, Guid UniqueId, string Username)>();

        }
        
        public async Task<AuthToken> GetTokenFromCache()
        {          
            return new AuthToken();
        }
        
        public async Task<ServiceResponse<bool>> Unauthenticate()
        {            
            return true.AsServiceResponse();
        }
    }
}
