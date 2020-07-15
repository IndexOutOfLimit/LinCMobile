using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Helpers;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.RestClient.Services
{   
    public class RestClientMSAL : RestClient, ITokenClient
    {
        private const string AuthSuffix = "ouch";

        private readonly IIDSConfiguration _idsConfiguration;
        private readonly IRestConfiguration _restConfiguration;
        private readonly IAdAuthService _adAuthService;
                
        public RestClientMSAL(IApiConfiguration configuration, IIDSConfiguration idsConfiguration, IRestConfiguration restConfiguration, IAdAuthService adAuthService)
            : base(configuration)
        {
            _idsConfiguration = idsConfiguration;
            _restConfiguration = restConfiguration;
            _adAuthService = adAuthService;

            SerializationSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };

            DeserializationSettings = SerializationSettings;
        }
               
        public override async Task<ServiceResponse<AuthToken>> GetAuthToken(
            string username, string password, AuthToken authToken, Dictionary<string, string> headers)
        {
            return CurrentAuthToken.AsServiceResponse();
        }

        public async Task<ServiceResponse<AuthToken>> GetAuthToken(string requestFor, Dictionary<string, string> extraRequest, Dictionary<string, string> headers)
        {
            return CurrentAuthToken.AsServiceResponse();
        }

        protected override async Task<(HttpRequestMessage httpRequestMessage, HttpClient httpClient)> PrepareRequest<TResult, TBody>(HttpVerb requestVerb, string action, string id, int amount, int start, string order,
            string[] includes, Dictionary<string, object> parameters, HttpParamMode paramMode, TBody requestBody, string apiRoutePrefix,
            Dictionary<string, string> headers = null, AuthToken authToken = null, bool isAnonymous = false, Dictionary<MetaData, string> metaData = null)
        {
            if (CurrentAuthToken == null)
            {
                var tokenResponse = await RefreshAuthToken();
                SetCurrentAuthToken(tokenResponse.Data);
            }
              
            return await base.PrepareRequest<TResult, TBody>(requestVerb, action, id, amount, start, order, includes, parameters, paramMode, requestBody, apiRoutePrefix, headers, authToken, isAnonymous, metaData);
        }
       
        public async Task<ServiceResponse<AuthToken>> RefreshAuthToken(Dictionary<string, string> headers = null)
        {
            var token = await _adAuthService.GetTokenFromCache();
            return token.AsServiceResponse();
        }
    }
}