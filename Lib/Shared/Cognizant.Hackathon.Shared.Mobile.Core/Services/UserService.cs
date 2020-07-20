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

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);

            if(jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<LinCUser>(ServiceStatus.Error, data: null, errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "User data not saved.");
            }

            var userResponse = JsonConvert.DeserializeObject<LinCUser>(jSonResponse);            

            return new ServiceResponse<LinCUser>(ServiceStatus.Success, data: userResponse, errorCode: LinCTrasactionStatus.Success.ToString());

        }

        public async Task<ServiceResponse<(LinCUser, bool)>> GetUserAsync(string deviceDensity, string deviceType, string companyCode, string userId, string UserCode)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            LinCUser newUser = new LinCUser
            {
                UserSecret = UserCode,
                Email = userId
            };
           
            var response = await _restClient
               .ExecuteAsync<string, LinCUser>(
                   HttpVerb.POST,
                   action: "GetUserInfo",
                   paramMode: HttpParamMode.BODY,
                   requestBody: newUser,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Problem in retrieving user data");

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Please enter valid credential.");
            }

            var userResponse = JsonConvert.DeserializeObject<LinCUser>(jSonResponse);
            return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Success, data: (userResponse, true));
        }

        public async Task<ServiceResponse<(LinCUser, bool)>> GetUserProducts(string userId, string UserCode)
        {
            var headers = RequestHeaderCreator.GetWebApiClientHeader();

            LinCUser newUser = new LinCUser
            {
                UserSecret = UserCode,
                Email = userId
            };

            var response = await _restClient
               .ExecuteAsync<string, LinCUser>(
                   HttpVerb.POST,
                   action: "/product/searchStoreOrProduct",
                   paramMode: HttpParamMode.BODY,
                   requestBody: newUser,
                   headers: headers,
                   apiRoutePrefix: $"{AppSettings.ApiEndpoint}"
                   );
            if (!response.IsOK() || string.IsNullOrEmpty(response.StringData))
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Problem in retrieving user data");

            var jSonResponse = response.StringData.Replace(@"\", string.Empty);
            if (jSonResponse.Contains("errorMessage"))
            {
                return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Error, data: (null, false), errorCode: LinCTrasactionStatus.Failure.ToString(), errorMessage: "Please enter valid credential.");
            }

            var userResponse = JsonConvert.DeserializeObject<LinCUser>(jSonResponse);
            return new ServiceResponse<(LinCUser, bool)>(ServiceStatus.Success, data: (userResponse, true));
        }
    }
}