using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Helpers;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.RestClient.Services
{   
    public class RestClientBasicAuth : RestClient
    {
        private const string AuthTokenType = "Basic";
                
        public RestClientBasicAuth(IApiConfiguration configuration) : base(configuration)
        {
            SerializationSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new ReadOnlyJsonContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new XmlTimeSpanConverter()
                }
            };

            DeserializationSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new ReadOnlyJsonContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new XmlTimeSpanConverter()
                }
            };
        }
       
        public override async Task<ServiceResponse<AuthToken>> GetAuthToken(
            string username, string password, AuthToken authToken, Dictionary<string, string> headers)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new UnauthorizedAccessException("Invalid credential");

            string tokenReponse = string.Empty;
            HttpResponseMessage response = null;

            var success = IsHostReachable();

            var sw = new Stopwatch();
            sw.Start();

            var tokenEndpointUrl = new Uri(Configuration.AuthUrl).ToString();

            using (var client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                try
                {
                    var data = $"{{\"email\":\"{username}\",\"password\":\"{password}\"}}";

                    var requestContent = new StringContent(data, Encoding.UTF8, MIME_JSON);

                    response = await client.PostAsync(tokenEndpointUrl, requestContent);
                    tokenReponse = await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    if (client != null) client.Dispose();
                    return new ServiceResponse<AuthToken>(ServiceStatus.Error, errorMessage: ex.Message, exception: ex);
                }
            }

            switch (response?.StatusCode)
            {
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.NotAcceptable:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.Unauthorized:

                    sw.Stop();

                    var errObj = JObject.Parse(tokenReponse);
                    var errorDesc = errObj["message"].ToString();

                    return new ServiceResponse<AuthToken>(ServiceStatus.Error,
                        errorMessage: $"Authentication Failed: {errorDesc}", errorType: ServiceErrorType.Authentication);
                default:
                    break;
            }

            var tokenObj = JObject.Parse(tokenReponse);

            long ttl = 31536000;

            var authTokenNew = Configuration.AuthUrl.Contains("user_auth_tokens")
                ? AuthTokenFactory.CreateBasicAuthToken(tokenObj["token"].ToString(), null, Convert.ToInt64(tokenObj["ttl"]))
                : AuthTokenFactory.CreateBasicAuthToken(tokenObj["email"].ToString(), tokenObj["api_key"].ToString(), ttl);
            
            SetCurrentAuthToken(authTokenNew);
            SetStatusCode(response.StatusCode);

            var tokenResponse = new ServiceResponse<AuthToken>(ServiceStatus.Success, data: authTokenNew)
            {
                ElapsedTime = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)
            };

            sw.Stop();

            return tokenResponse;
        }
    }

}
