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
using Cognizant.Hackathon.Shared.Mobile.Core.Enums;

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

        public async Task<ServiceResponse<LinCUser>> CreateUserAsync(string deviceDensity, string deviceType, LinCUser newUser)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            var response = await _restClient
               .ExecuteAsync<string, LinCUser>(
                   HttpVerb.POST,
                   action: "/user/registration",
                   paramMode: HttpParamMode.BODY,
                   requestBody: newUser,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<LinCUser>(ServiceStatus.Error, data: null,errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "User data not saved.");
            var jSonResponse = response.StringData.Substring(1, response.StringData.Length - 2);
            jSonResponse = jSonResponse.Replace(@"\", string.Empty);

            ResponseInfo<Error> errorResponse = JsonConvert.DeserializeObject<ResponseInfo<Error>>(jSonResponse);

            var UserResponse = JsonConvert.DeserializeObject<ResponseInfo<LinCUser>>(jSonResponse);

            if (UserResponse.Message.Body.UserCode.Equals("Error:0000"))
            {
                return new ServiceResponse<LinCUser>(ServiceStatus.Error, data: null, message: "Error:0000", errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "User data not saved.");
            }

            return new ServiceResponse<LinCUser>(ServiceStatus.Success, data: UserResponse.Message.Body, errorCode: LinCTrasactionStatus.Success.ToString());

        }

        public async Task<ServiceResponse<(LinCUser, bool)>> GetUserAsync(string deviceDensity, string deviceType, string companyCode, string userId, string UserCode)
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
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Error, data: (null, false), errorMessage: "Problem in retrieving user data");

            var jSonResponse = response.StringData.Substring(1, response.StringData.Length - 2);
            jSonResponse = jSonResponse.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("AppVersionError"))
            {
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Success, data: (null, true), errorMessage: "AppVersionError");
            }

            // var UserResponse = JsonConvert.DeserializeObject<ResponseInfo<UserResponse>>(jSonResponse);

            var UserResponse = JsonConvert.DeserializeObject<ResponseInfo<LinCUser>>(jSonResponse);
            var serviceresponse = new ServiceResponse<LinCUser>(ServiceStatus.Success, data: UserResponse.Message.Body);
            return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Success, data: (UserResponse.Message.Body, false));
            // return new ServiceResponse<User>(ServiceStatus.Error);//, data: null, errorMessage: "No Data");
        }
    }
}