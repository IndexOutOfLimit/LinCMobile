using System;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.RestClient.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Request;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IRestClient _restClient;

        public UserService(IRestClient restClient)
        {
            _restClient = restClient;
        }
        public async Task<ServiceResponse<LinCUser>> GetOrCreateUser(Guid userId, string accessToken = null)
        {
            var response = await RestClient
                .ExecuteAsync<LinCUser, Guid>(
                    HttpVerb.GET,
                    nameof(GetOrCreateUser),
                    isServiceResponse: true,
                    apiRoutePrefix: $"{AppSettings.ApiEndpoint}{nameof(LinCUser)}/");

            return response;
        }

        public async Task<ServiceResponse<UserReqBody>> CreateUserAsync(string deviceDensity, string deviceType, LinCUser newUser)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

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
               .ExecuteAsync<string, LinCUser>(
                   HttpVerb.POST,
                   action: "UserDetailsSave",
                   paramMode: HttpParamMode.BODY,
                   requestBody: newUser,
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

        public async Task<ServiceResponse<(UserReqBody, bool)>> GetUserAsync(string deviceDensity, string deviceType, string companyCode, string userId, string UserCode)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            LinCUser newUser = new LinCUser
            {
                UserCode = UserCode,
                UserId = userId
            };
            var reqObject = new RequestInfo<UserReqBody>
            {
                Message = new RequestMessege<UserReqBody>
                {
                    Header = RequestHeaderCreator.GetRequestHeader(deviceDensity, deviceType, companyCode, UserCode, userId),
                    Body = new UserReqBody
                    {
                        UserInfo = newUser
                    }
                }
            };
            var response = await _restClient
               .ExecuteAsync<string, RequestInfo<UserReqBody>>(
                   HttpVerb.POST,
                   action: "GetUserInfo",
                   paramMode: HttpParamMode.BODY,
                   requestBody: reqObject,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<(UserReqBody, bool)>(ServiceStatus.Error, data: (null, false), errorMessage: "Problem in retrieving user data");

            var jSonResponse = response.StringData.Substring(1, response.StringData.Length - 2);
            jSonResponse = jSonResponse.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("AppVersionError"))
            {
                return new ServiceResponse<(UserReqBody, bool)>(ServiceStatus.Success, data: (null, true), errorMessage: "AppVersionError");
            }

            // var UserResponse = JsonConvert.DeserializeObject<ResponseInfo<UserResponse>>(jSonResponse);

            var UserResponse = JsonConvert.DeserializeObject<ResponseInfo<UserReqBody>>(jSonResponse);
            var serviceresponse = new ServiceResponse<UserReqBody>(ServiceStatus.Success, data: UserResponse.Message.Body);
            return new ServiceResponse<(UserReqBody, bool)>(ServiceStatus.Success, data: (UserResponse.Message.Body, false));
            // return new ServiceResponse<User>(ServiceStatus.Error);//, data: null, errorMessage: "No Data");
        }
    }
}