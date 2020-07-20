using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Helpers;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Request;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly IUserService _userService;
        private readonly IAdAuthService _adAuthService;
        private readonly IRestClient _restClient;

        public AuthService(IUserService userService, IAdAuthService adAuthService, IRestClient restClient)
        {
            _userService = userService;
            _adAuthService = adAuthService;
            _restClient = restClient;
        }

        public async Task<ServiceResponse<bool>> Login()
        {
            // get token
            var resultAuth = await _adAuthService.Authenticate(CrossCurrentActivity.Current);
            
            if (resultAuth == null)
                return false.AsServiceResponse();
            
            // Crucial part : tie up with rest client
            _restClient.SetCurrentAuthToken(resultAuth.Data.authToken);
            
            var resultUser = await _userService.GetOrCreateUser(resultAuth.Data.UniqueId);

            if (!resultUser.IsOK() || resultUser.Data == null)
                return resultUser.ToServiceResponse<LinCUser, bool>();

            AppCache.State.Member = resultUser.Data;
            
            //AppCache.State.Credential.AuthToken = resultAuth.Data.authToken;
            //AppCache.State.Credential.Username = resultAuth.Data.Username;
            //AppCache.State.Credential.UserID = resultAuth.Data.UniqueId;
            
            await AppCache.Save();

            return true.AsServiceResponse();
        }

        public async Task<ServiceResponse<UserReqBody>> Login(string userId, string pwd)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();
            LinCUser newUser = new LinCUser() { Email = userId, UserSecret = pwd };
            var reqObject = new RequestInfo<UserReqBody>
            {
                Message = new RequestMessege<UserReqBody>
                {
                    //Header = RequestHeaderCreator.GetRequestHeader(deviceDensity, deviceType, newUser.CompanyCode, newUser.UserCode, newUser.UserId),
                    Body = new UserReqBody
                    {
                        UserInfo = newUser
                    }
                }
            };

            var response = await _restClient
               .ExecuteAsync<string, RequestInfo<UserReqBody>>(
                   HttpVerb.POST,
                   action: "UserDetailsSave",
                   paramMode: HttpParamMode.BODY,
                   requestBody: reqObject,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<UserReqBody>(ServiceStatus.Error, data: null, errorMessage: "User data not saved.");
            var jSonResponse = response.StringData.Substring(1, response.StringData.Length - 2);
            jSonResponse = jSonResponse.Replace(@"\", string.Empty);

            ResponseInfo<Error> errorResponse = JsonConvert.DeserializeObject<ResponseInfo<Error>>(jSonResponse);

            var UserResponse = JsonConvert.DeserializeObject<ResponseInfo<UserReqBody>>(jSonResponse);

            if (UserResponse.Message.Body.UserInfo.UserCode.Equals("Error:0000"))
            {
                return new ServiceResponse<UserReqBody>(ServiceStatus.Error, data: null, message: "Error:0000", errorMessage: "User data not saved.");
            }

            return new ServiceResponse<UserReqBody>(ServiceStatus.Success, data: UserResponse.Message.Body);

        }

        public async Task<ServiceResponse<bool>> Logout()
        {
            if (AppCache.State.IsAuthenticated)
            {
                await _adAuthService.Unauthenticate();

                await AppCache.Clear();
            }

            return new ServiceResponse<bool>(ServiceStatus.Success, data: true);
        }
    }
}