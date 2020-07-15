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
    public class RestClientIDS : RestClient, ITokenClient
    {
        private const string AuthSuffix = "ouch";

        private readonly IIDSConfiguration _idsConfiguration;
        private readonly IRestConfiguration _restConfiguration;

       public RestClientIDS(IApiConfiguration configuration, IIDSConfiguration idsConfiguration, IRestConfiguration restConfiguration)
            : base(configuration)
        {
            _idsConfiguration = idsConfiguration;
            _restConfiguration = restConfiguration;

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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new UnauthorizedAccessException("Invalid credential");

            TokenResponse response = null;

            string clientId = _idsConfiguration.IdentityClientId;
            string clientScope = _idsConfiguration.IdentityScope;
            string clientSecret = _idsConfiguration.IdentityClientSecret;

            if (string.IsNullOrEmpty(clientId)
                || string.IsNullOrEmpty(clientScope)
                || string.IsNullOrEmpty(clientSecret))
                throw new UnauthorizedAccessException("Invalid credential");
            
            var success = IsHostReachable();

            var sw = new Stopwatch();
            sw.Start();

            var tokenEndpoint = _restConfiguration.UrlRewriteProvider.Rewrite($"{AuthSuffix}/connect/token");
            var tokenEndpointUrl = new Uri(_apiClient.BaseAddress, tokenEndpoint).ToString();

            using (var client = new HttpClient())
            {
                if (headers != null)
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);

                var parameters = new Dictionary<string, string> { { IdentityModel.OidcConstants.AuthorizeRequest.Scope, clientScope } };

                try
                {
                    response = await client.RequestPasswordTokenAsync(
                        new PasswordTokenRequest
                        {
                            Address = tokenEndpointUrl,
                            ClientId = clientId,
                            GrantType = IdentityModel.OidcConstants.GrantTypes.Password,
                            ClientSecret = clientSecret,
                            UserName = username,
                            Password = password,
                            Parameters = parameters
                        });
                }
                catch (Exception ex)
                {
                    if (client != null) client.Dispose();
                    return new ServiceResponse<AuthToken>(ServiceStatus.Error, errorMessage: ex.Message, exception: ex);
                }
            }

            switch (response?.HttpStatusCode)
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

                    var error = response.TryGet(IdentityModel.OidcConstants.TokenResponse.Error);
                    var errorDescription = response.TryGet(IdentityModel.OidcConstants.TokenResponse.ErrorDescription);

                    sw.Stop();

                    if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(errorDescription))
                    {
                        return new ServiceResponse<AuthToken>(ServiceStatus.Error,
                            errorMessage: $"Authentication Failed", errorType: ServiceErrorType.Authentication);
                    }
                    else
                    {
                        return new ServiceResponse<AuthToken>(ServiceStatus.Error,
                            message: error, errorMessage: errorDescription, errorType: ServiceErrorType.Authentication);
                    }

                default:
                    break;
            }

            var issuedAt = DateTime.UtcNow;

            var authTokenNew = new AuthToken
            {
                IssuedAt = issuedAt,
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                TokenType = response.TokenType,
                ExpiresIn = response.ExpiresIn,
                ExpiresAt = issuedAt.AddSeconds(response.ExpiresIn),
                ApiVersion = response.TryGet(AuthToken.ApiVersionKey),
                MinMobileAppVersion = response.TryGet(AuthToken.MinMobileAppVersionKey)
            };

            SetCurrentAuthToken(authTokenNew);
            SetStatusCode(response.HttpStatusCode);

            var tokenResponse = new ServiceResponse<AuthToken>(ServiceStatus.Success, data: authTokenNew)
            {
                ElapsedTime = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)
            };

            sw.Stop();

            return tokenResponse;
        }

        private ServiceResponse<AuthToken> GetAuthTokenResponseError(TokenResponse response, string requestFor)
        {
            bool isError = false;

            switch (response?.HttpStatusCode)
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
                    isError = true;
                    break;

                default:
                    if (response.ErrorType == ResponseErrorType.Exception)
                        isError = true;
                    break;
            }

            if (!isError) return null;

            var error = response.TryGet(IdentityModel.OidcConstants.TokenResponse.Error);
            var errorDescription = response.TryGet(IdentityModel.OidcConstants.TokenResponse.ErrorDescription);

            if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(errorDescription))
            {
                return new ServiceResponse<AuthToken>(ServiceStatus.Error,
                    errorMessage: $"Failed executing for {requestFor}", errorType: ServiceErrorType.Authentication);
            }
            else
            {
                return new ServiceResponse<AuthToken>(ServiceStatus.Error,
                    error, errorDescription, errorType: ServiceErrorType.Authentication);
            }
        }

       
        public async Task<ServiceResponse<AuthToken>> GetAuthToken(string requestFor, Dictionary<string, string> extraRequest, Dictionary<string, string> headers)
        {
            if (string.IsNullOrEmpty(requestFor))
                throw new UnauthorizedAccessException("Invalid action");

            TokenResponse response = null;

            string clientId = string.Empty;
            string clientScope = string.Empty;
            string clientSecret = string.Empty;

            if (string.IsNullOrEmpty(clientId)
                || string.IsNullOrEmpty(clientScope)
                || string.IsNullOrEmpty(clientSecret))
                throw new UnauthorizedAccessException("Invalid credential");
            
            IsHostReachable();

            var sw = new Stopwatch();
            sw.Start();

            var tokenEndpoint = _restConfiguration.UrlRewriteProvider.Rewrite($"{AuthSuffix}/connect/token");
            var tokenEndpointUrl = new Uri(_apiClient.BaseAddress, tokenEndpoint).ToString();

            using (var client = new HttpClient())
            {
                if (headers != null)
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);

                var parameters = new Dictionary<string, string> { { IdentityModel.OidcConstants.AuthorizeRequest.Scope, clientScope } };

                if (extraRequest != null)
                    foreach (var item in extraRequest)
                        parameters.Add(item.Key, item.Value);

                try
                {
                    response = await client.RequestTokenAsync(new TokenRequest
                    {
                        Address = tokenEndpointUrl,
                        ClientId = clientId,
                        GrantType = IdentityModel.OidcConstants.GrantTypes.ClientCredentials,
                        ClientSecret = clientSecret,
                        Parameters = parameters
                    });
                }
                catch (Exception ex)
                {
                    if (client != null) client.Dispose();
                    return new ServiceResponse<AuthToken>(ServiceStatus.Error, errorMessage: ex.Message, exception: ex);
                }
            }

            var responseError = GetAuthTokenResponseError(response, requestFor);

            sw.Stop();

            if (responseError != null)
                return responseError;

            var issuedAt = DateTime.UtcNow;

            var authTokenNew = new AuthToken
            {
                IssuedAt = issuedAt,
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                TokenType = response.TokenType,
                ExpiresIn = response.ExpiresIn,
                ExpiresAt = issuedAt.AddSeconds(response.ExpiresIn),
                ApiVersion = response.TryGet(AuthToken.ApiVersionKey),
                MinMobileAppVersion = response.TryGet(AuthToken.MinMobileAppVersionKey)
            };

            SetStatusCode(response.HttpStatusCode);

            var tokenResponse = new ServiceResponse<AuthToken>(ServiceStatus.Success, data: authTokenNew)
            {
                ElapsedTime = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)
            };

            return tokenResponse;
        }        

        public async Task<ServiceResponse<AuthToken>> RefreshAuthToken(Dictionary<string, string> headers)
        {
            if (CurrentAuthToken is null)
                throw new UnauthorizedAccessException("Authentication failed!");

            var refreshToken = CurrentAuthToken.RefreshToken;

            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException("Invalid refresh token");

            string clientId = _idsConfiguration.IdentityClientId;
            string clientScope = _idsConfiguration.IdentityScope;
            string clientSecret = _idsConfiguration.IdentityClientSecret;

            if (string.IsNullOrEmpty(clientId)
                || string.IsNullOrEmpty(clientScope)
                || string.IsNullOrEmpty(clientSecret))
                throw new UnauthorizedAccessException("Invalid credential");
            
            var success = IsHostReachable();

            var sw = new Stopwatch();
            sw.Start();

            var tokenEndpoint = _restConfiguration.UrlRewriteProvider.Rewrite($"{AuthSuffix}/connect/token");
            var tokenEndpointUrl = new Uri(_apiClient.BaseAddress, tokenEndpoint).ToString();

            var client = new HttpClient();

            foreach (var header in headers)
                client.DefaultRequestHeaders.Add(header.Key, header.Value);

            var parameters = new Dictionary<string, string> { { IdentityModel.OidcConstants.AuthorizeRequest.Scope, clientScope } };

            var response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = tokenEndpointUrl,
                ClientId = clientId,
                GrantType = IdentityModel.OidcConstants.GrantTypes.RefreshToken,
                ClientSecret = clientSecret,
                RefreshToken = refreshToken,
                Parameters = parameters
            });

            client.Dispose();

            switch (response?.HttpStatusCode)
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

                    var error = response.TryGet(IdentityModel.OidcConstants.TokenResponse.Error);
                    var errorDescription = response.TryGet(IdentityModel.OidcConstants.TokenResponse.ErrorDescription);

                    sw.Stop();

                    if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(errorDescription))
                    {
                        return new ServiceResponse<AuthToken>(ServiceStatus.Error,
                            errorMessage: $"Authentication failed!", errorType: ServiceErrorType.Authentication);
                    }
                    else
                    {
                        return new ServiceResponse<AuthToken>(ServiceStatus.Error,
                            message: error, errorMessage: errorDescription, errorType: ServiceErrorType.Authentication);
                    }

                default:
                    break;
            }

            var issuedAt = DateTime.UtcNow;

            var authTokenNew = new AuthToken
            {
                IssuedAt = issuedAt,
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                TokenType = response.TokenType,
                ExpiresIn = response.ExpiresIn,
                ExpiresAt = issuedAt.AddSeconds(response.ExpiresIn),
                ApiVersion = response.TryGet(AuthToken.ApiVersionKey),
                MinMobileAppVersion = response.TryGet(AuthToken.MinMobileAppVersionKey)
            };

            SetCurrentAuthToken(authTokenNew);
            SetStatusCode(response.HttpStatusCode);

            var tokenRefreshResponse = new ServiceResponse<AuthToken>(ServiceStatus.Success, data: authTokenNew)
            {
                ElapsedTime = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)
            };

            sw.Stop();

            return tokenRefreshResponse;
        }
    }
}