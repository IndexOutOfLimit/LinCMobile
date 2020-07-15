using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Request;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces
{
    public interface IAuthService
    {
        #region Sign API

        Task<ServiceResponse<bool>> Login();
        Task<ServiceResponse<UserReqBody>> Login(string userId, string pwd);
        Task<ServiceResponse<bool>> Logout();

        #endregion
        
    }
}
