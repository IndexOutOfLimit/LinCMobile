using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Cognizant.Hackathon.RestClient.Helpers;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.RestClient.Providers;

namespace Cognizant.Hackathon.RestClient.Services
{    
    public abstract class RestClient : IRestClient
    {
        #region Constants       
        internal const string MIME_JSON = "application/json";
        internal const string MIME_BSON = "application/bson";
        internal const string MIME_OCTET_STREAM = "application/octet-stream";
        internal const string MIME_XFORM = "application/x-www-form-urlencoded";
        internal const int MAX_NETWORK_FAILURE = 5;
        #endregion Constants

        #region Fields
        protected readonly HttpClient _apiClient;
        private readonly HttpClient _streamClient;
        private readonly HttpClient _miscClient;
        #endregion Fields

        #region Public Properties
      
        public IApiConfiguration Configuration { get; }
     
        public Uri BaseUri { get; }
       
        public double ApiTimeout { get; }
       
        public JsonSerializerSettings SerializationSettings { get; internal set; }
       
        public JsonSerializerSettings DeserializationSettings { get; internal set; }
        
        public int StatusCode { get; private set; }
        
        public AuthToken CurrentAuthToken { get; private set; }

        #endregion Public Properties

        #region Constructors and Destructor
        
        public RestClient(IApiConfiguration configuration)
        {
            Configuration = configuration;

            BaseUri = new Uri(configuration.BaseUrl);
            ApiTimeout = configuration.ApiTimeout;

            _apiClient = new HttpClient()
            {
                BaseAddress = BaseUri,
                Timeout = TimeSpan.FromMilliseconds(ApiTimeout)
            };

            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MIME_JSON));

            _streamClient = new HttpClient()
            {
                BaseAddress = BaseUri,
                Timeout = TimeSpan.FromMilliseconds(ApiTimeout)
            };

            _streamClient.DefaultRequestHeaders.Accept.Clear();
            _streamClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MIME_OCTET_STREAM));

            _miscClient = new HttpClient();
            _miscClient.Timeout = TimeSpan.FromMilliseconds(ApiTimeout);
            _miscClient.DefaultRequestHeaders.Accept.Clear();
        }

        #endregion Constructors and Destructor

        #region Public Methods and Operators

        public ServiceResponse<T> Execute<T>(
            HttpVerb requestVerb,
            [CallerMemberName] string action = null,
            T requestBody = default(T),
            string id = null,
            int amount = -1,
            int start = -1,
            string order = "",
            string[] includes = null,
            Dictionary<string, object> parameters = null,
            HttpParamMode paramMode = HttpParamMode.REST,
            string apiRoutePrefix = null,
            Dictionary<string, string> headers = null,
            AuthToken authToken = null,
            bool isServiceResponse = false,
            bool isAnonymous = false,
            bool skipHostCheck = false,
            Dictionary<MetaData, string> metaData = null)
        {
            return Execute<T, T>(
                requestVerb,
                action,
                requestBody,
                id,
                amount,
                start,
                order,
                includes,
                parameters,
                paramMode,
                apiRoutePrefix,
                headers,
                authToken,
                isServiceResponse,
                isAnonymous,
                skipHostCheck,
                metaData);
        }
       
        public ServiceResponse<TResult> Execute<TResult, TBody>(HttpVerb requestVerb, [CallerMemberName] string action = null,
            TBody requestBody = default(TBody), string id = null, int amount = -1, int start = -1, string order = "",
            string[] includes = null, Dictionary<string, object> parameters = null, HttpParamMode paramMode = HttpParamMode.REST,
            string apiRoutePrefix = null, Dictionary<string, string> headers = null, AuthToken authToken = null,
            bool isServiceResponse = false, bool isAnonymous = false, bool skipHostCheck = false, Dictionary<MetaData, string> metaData = null)
        {
            return GetServiceResponseAsync<TResult, TBody>(
                    requestVerb,
                    action,
                    requestBody,
                    id,
                    amount,
                    start,
                    order,
                    includes,
                    parameters,
                    paramMode,
                    apiRoutePrefix,
                    headers,
                    authToken,
                    isServiceResponse,
                    isAnonymous,
                    skipHostCheck,
                    metaData).Result;
        }

        public async Task<ServiceResponse<T>> ExecuteAsync<T>(HttpVerb requestVerb, [CallerMemberName] string action = null,
            T requestBody = default(T), string id = null, int amount = -1, int start = -1, string order = "", string[] includes = null,
            Dictionary<string, object> parameters = null, HttpParamMode paramMode = HttpParamMode.REST, string apiRoutePrefix = null,
            Dictionary<string, string> headers = null, AuthToken authToken = null, bool isServiceResponse = false,
            bool isAnonymous = false, bool skipHostCheck = false, Dictionary<MetaData, string> metaData = null)
        {
            return await GetServiceResponseAsync<T, T>(
                    requestVerb,
                    action,
                    requestBody,
                    id,
                    amount,
                    start,
                    order,
                    includes,
                    parameters,
                    paramMode,
                    apiRoutePrefix,
                    headers,
                    authToken,
                    isServiceResponse,
                    isAnonymous,
                    skipHostCheck,
                    metaData);
        }
       
        public async Task<ServiceResponse<TResult>> ExecuteAsync<TResult, TBody>(
            HttpVerb requestVerb,
            [CallerMemberName] string action = null,
            TBody requestBody = default(TBody),
            string id = null,
            int amount = -1,
            int start = -1,
            string order = "",
            string[] includes = null,
            Dictionary<string, object> parameters = null,
            HttpParamMode paramMode = HttpParamMode.REST,
            string apiRoutePrefix = null,
            Dictionary<string, string> headers = null,
            AuthToken authToken = null,
            bool isServiceResponse = false,
            bool isAnonymous = false,
            bool skipHostCheck = false,
            Dictionary<MetaData, string> metaData = null)
        {
            return await
                GetServiceResponseAsync<TResult, TBody>(
                    requestVerb,
                    action,
                    requestBody,
                    id,
                    amount,
                    start,
                    order,
                    includes,
                    parameters,
                    paramMode,
                    apiRoutePrefix,
                    headers,
                    authToken,
                    isServiceResponse,
                    isAnonymous,
                    skipHostCheck,
                    metaData);
        }

        #endregion Public Methods and Operators

        #region Methods

        private static HttpMethod HttpVerb2Method(HttpVerb verb)
        {
            switch (verb)
            {
                case HttpVerb.DELETE:
                    return HttpMethod.Delete;

                default:
                case HttpVerb.GET:
                    return HttpMethod.Get;

                case HttpVerb.HEAD:
                    return HttpMethod.Head;

                case HttpVerb.OPTIONS:
                    return HttpMethod.Options;
              
                case HttpVerb.POST:
                    return HttpMethod.Post;

                case HttpVerb.PUT:
                    return HttpMethod.Put;
            }
        }
       
        public abstract Task<ServiceResponse<AuthToken>> GetAuthToken(
            string username,
            string password,
            AuthToken authToken,
            Dictionary<string, string> headers);
       
        private string GetFormattedErrorMessage(string error, string requestUri, object value)
        {
            if (value is StreamContent)
                value = "";

            return $"Error '{error}' getting JSON with status {StatusCode} for resource '{requestUri}' : {value}";
        }

        private async Task<ServiceResponse<TResult>> GetServiceResponseAsync<TResult, TBody>(
            HttpVerb requestVerb,
            string action,
            TBody requestBody,
            string id = null,
            int amount = -1,
            int start = -1,
            string order = "",
            string[] includes = null,
            Dictionary<string, object> parameters = null,
            HttpParamMode paramMode = HttpParamMode.REST,
            string apiRoutePrefix = null,
            Dictionary<string, string> headers = null,
            AuthToken authToken = null,
            bool isServiceResponse = false,
            bool isAnonymous = false,
            bool skipHostCheck = false,
            Dictionary<MetaData, string> metaData = null)
        {
            if (!skipHostCheck)
            {
                var success = IsHostReachable();

                if (!success)
                {
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.None)
                        throw new NetworkException("Network not available");
                }
            }

            var (httpRequestMessage, httpClient) = await PrepareRequest<TResult, TBody>(
                requestVerb, action, id, amount, start, order, includes, parameters, paramMode, requestBody, apiRoutePrefix, headers, authToken, isAnonymous, metaData);

            return await SendRequest<TResult>(httpClient, httpRequestMessage, isServiceResponse);
        }

        protected bool IsHostReachable()
        {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.None)
                    throw new NetworkException("Network not available");

                return true;
        }

        protected async virtual Task<(HttpRequestMessage httpRequestMessage, HttpClient httpClient)>
            PrepareRequest<TResult, TBody>(HttpVerb requestVerb, string action, string id, int amount, int start,
                string order, string[] includes,
                Dictionary<string, object> parameters, HttpParamMode paramMode, TBody requestBody,
                string apiRoutePrefix,
                Dictionary<string, string> headers = null, AuthToken authToken = null, bool isAnonymous = false,
                Dictionary<MetaData, string> metaData = null)
        {
            HttpClient client = null;

           if (metaData != null && metaData.ContainsKey(MetaData.ParamMode))
                Enum.TryParse(metaData[MetaData.ParamMode], out paramMode);

            var resource = new StringBuilder();
            Type bodyType = typeof(TBody);
            Type resultType = typeof(TResult);

            bool isMiscClient = false;
            if (!string.IsNullOrEmpty(apiRoutePrefix))
            {
                if (apiRoutePrefix.ToLower().Contains("http"))
                {
                    client = _miscClient;

                    if(client.BaseAddress == null)
                        client.BaseAddress = new Uri(apiRoutePrefix);

                    isMiscClient = true;
                }
                else
                {
                    client = resultType != typeof(Stream) ? _apiClient : this._streamClient;
                    if (!string.IsNullOrEmpty(apiRoutePrefix))
                        resource.Append(apiRoutePrefix);
                }
            }
            else
            {
                client = resultType != typeof(Stream) ? _apiClient : this._streamClient;

                if (bodyType.GenericTypeArguments != null && bodyType.GenericTypeArguments.Length == 1)
                    resource.Append(bodyType.GenericTypeArguments[0].Name);
                else
                    resource.Append(bodyType.Name);
            }

            if (!string.IsNullOrEmpty(action)) resource.Append("/" + action);
            if (!string.IsNullOrEmpty(id)) resource.Append("/" + id);

            if (parameters != null && parameters.Any())
            {
                string p = null;
                switch (paramMode)
                {
                    case HttpParamMode.QUERYSTRING:
                        p = $"?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value.ToString()).Replace("+", " ")}"))}";
                        break;

                    case HttpParamMode.REST:
                        p = $"/{string.Join("/", parameters.Select(kvp => $"{kvp.Key}/{WebUtility.UrlEncode(kvp.Value.ToString()).Replace("+", " ")}"))}";
                        break;

                    case HttpParamMode.RESTVALUE:
                        p = $"/{string.Join("/", parameters.Select(kvp => $"{WebUtility.UrlEncode(kvp.Value.ToString()).Replace("+", " ")}"))}";
                        break;
                }

                if (p != null) resource.Append(p);
            }

            if (client.BaseAddress.AbsolutePath.EndsWith("/") && resource.ToString().StartsWith("/"))
                resource.Remove(0, 1);

            var requestUrl = new Uri(client.BaseAddress, resource.ToString());
            var request = new HttpRequestMessage(HttpVerb2Method(requestVerb), requestUrl);

            request.Headers.Clear();
            try
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        try
                        {
                            request.Headers.Remove(header.Key);
                            request.Headers.Add(header.Key, header.Value);
                        }
                        catch (Exception ex)
                        {

                        }                        
                    }

                    if (paramMode == HttpParamMode.XFORM || paramMode == HttpParamMode.MULTIPART)
                    {
                        if (!request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue(MIME_XFORM)))
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MIME_XFORM));
                    }
                    else
                    {
                        if (!request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue(MIME_JSON)))
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MIME_JSON));
                    }
                }
                else
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MIME_JSON));
                }
            }
            catch (Exception)
            {
               
            }

            if (authToken is null)
            {
                if (!isMiscClient || (CurrentAuthToken != null && !string.IsNullOrWhiteSpace(CurrentAuthToken.AccessToken)) )
                {                    
                    request.Headers.Authorization =
                        new AuthenticationHeaderValue(CurrentAuthToken.TokenType, CurrentAuthToken.AccessToken);
                }
            }
            else
            {
                if (!isMiscClient)
                {                    
                    request.Headers.Authorization =
                            new AuthenticationHeaderValue(authToken.TokenType, authToken.AccessToken);
                }
            }
            
            if (parameters != null && parameters.Any() && paramMode == HttpParamMode.BODY)
            {               
                string data = JsonConvert.SerializeObject(parameters, SerializationSettings);
                request.Content = new StringContent(data, Encoding.UTF8, MIME_JSON);
            }

            if (parameters != null && parameters.Any() && paramMode == HttpParamMode.MULTIPART)
            {
                var multipartData = parameters.Values.Single(x => x.GetType() == typeof(byte[]));

                if (multipartData != null)
                {
                    var fileStreamContent = new ByteArrayContent(multipartData as byte[]);

                    string mimeType = "image/jpeg";

                    if (metaData != null && metaData.ContainsKey(MetaData.MimeType))
                        mimeType = metaData[MetaData.MimeType];

                    string fileName = Guid.NewGuid().ToString().Trim('{', '}');

                    if (metaData != null && metaData.ContainsKey(MetaData.FileName))
                        fileName = metaData[MetaData.FileName];

                    fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                    var formData = new MultipartFormDataContent { { fileStreamContent, "file" } };

                    foreach (var parameter in parameters)
                    {
                        if (parameter.Value.GetType() == typeof(byte[]))
                            continue;

                        if (!parameter.Value.GetType().IsPrimitive)
                            formData.Add(new StringContent(JsonConvert.SerializeObject(parameter.Value)), parameter.Key);
                        else
                            formData.Add(new StringContent(parameter.Value.ToString()), parameter.Key);
                    }

                    request.Content = formData;
                }
                else
                {
                    throw new Exception("There is no media content in the parameter");
                }
            }

            if (parameters != null && parameters.Any() && paramMode == HttpParamMode.XFORM)
            {
                var data1 = parameters.ToDictionary(p => p.Key, p => p.Value?.ToString());
                request.Content = new FormUrlEncodedContent(data1);
            }
            else if ((typeof(TBody).GetTypeInfo().IsValueType && !Equals(requestBody, default(TBody)))
                || (!typeof(TBody).GetTypeInfo().IsValueType && requestBody != null))
            {
                string data2 = JsonConvert.SerializeObject(requestBody, SerializationSettings);
                request.Content = new StringContent(data2, Encoding.UTF8, MIME_JSON);
            }
            
            return (request, client);
        }
      
        private async Task<ServiceResponse<TResult>> ProcessHttpResponse<TResult>(HttpResponseInfo info, bool isServiceResponse)
        {
            ServiceResponse<TResult> data;
            TimeSpan elapsedTime = DateTime.UtcNow - info.StartTime;

            Debug.WriteLine("[{0}ms] rest roundtrip", elapsedTime.TotalMilliseconds);

            HttpResponseMessage response = info.Response;

            if (info.Exception != null && response != null)
            {
                Debug.WriteLine(info.Exception.GetInnermostExceptionMessage());
                StatusCode = (int)response.StatusCode;
                string message = GetFormattedErrorMessage(
                    response.ReasonPhrase,
                    response.RequestMessage.RequestUri.ToString(),
                    response.Content);
                return new ServiceResponse<TResult>(ServiceStatus.Error, "Error processing request", message)
                { ElapsedTime = elapsedTime, StatusCode = StatusCode };
            }

            if (info.Exception != null && response == null)
            {
                string message = info.Exception.GetInnermostExceptionMessage();
                return new ServiceResponse<TResult>(ServiceStatus.Error, "Error processing request", message) { ElapsedTime = elapsedTime };
            }

            if (info.Exception == null && response == null)
            {
                string message = "No response received from server";
                return new ServiceResponse<TResult>(ServiceStatus.Error, message) { ElapsedTime = elapsedTime };
            }

            if (response.RequestMessage.Method == HttpMethod.Get && response.StatusCode != HttpStatusCode.OK
                || new[] { HttpMethod.Put, HttpMethod.Post }.Contains(response.RequestMessage.Method)
                && new[] { HttpStatusCode.OK, HttpStatusCode.NoContent }.Contains(response.StatusCode) == false)
            {
                StatusCode = (int)response.StatusCode;
                string message = GetFormattedErrorMessage(
                    response?.ReasonPhrase,
                    response?.RequestMessage?.RequestUri.ToString(),
                    response?.Content);


                string stringData = null;
                try
                {
                    var s = await response.Content?.ReadAsStreamAsync();
                    stringData = s.StreamToString();
                }
                catch
                {                    
                }

                return new ServiceResponse<TResult>(ServiceStatus.Error, "Bad response received from server", message, stringData: stringData) { StatusCode = StatusCode };
            }

            HttpContent content = response.Content;

            string json = string.Empty;

            try
            {
                if (typeof(TResult) == typeof(Stream) && content.Headers.ContentDisposition.Equals(new ContentDispositionHeaderValue("attachment")))
                {
                    var stream = await content.ReadAsStreamAsync();
                    data = new ServiceResponse<TResult>(true) { Stream = stream };
                }
                else if (typeof(TResult) == typeof(byte[]))
                {
                    var bytes = await content.ReadAsByteArrayAsync();
                    data = new ServiceResponse<TResult>(true) { Bytes = bytes };
                }
                else
                {
                    try
                    {
                        var s = await content.ReadAsStreamAsync();
                        json = s.StreamToString();
                    }
                    catch { throw; }

                    var errorContentCheck = CheckForErrorContent<TResult>(json);
                    if (errorContentCheck != null)
                        return errorContentCheck;

                    if (!isServiceResponse)
                    {
                        if (typeof(TResult) == typeof(string))
                        {
                            try
                            {
                                data = new ServiceResponse<TResult>(ServiceStatus.Success, stringData: json);
                            }
                            catch { throw; }
                        }
                        else
                        {
                            data = JsonConvert.DeserializeObject<TResult>(json, DeserializationSettings)
                                .AsServiceResponse();
                        }
                    }
                    else
                    {
                        try
                        {
                            data = JsonConvert.DeserializeObject<ServiceResponse<TResult>>(json, DeserializationSettings);
                        }
                        catch { throw; }
                    }
                }

                data.ElapsedTime = elapsedTime;
                data.RequestDateTime = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                string message = GetFormattedErrorMessage(
                    ex.GetInnermostExceptionMessage(),
                    response.RequestMessage.RequestUri.ToString(),
                    json);

                return new ServiceResponse<TResult>(ServiceStatus.Error, "Error deserializing response data", message);
            }

            StatusCode = (int)response.StatusCode;
            return data;
        }

        private ServiceResponse<TResult> CheckForErrorContent<TResult>(string json)
        {
            var errors = new[] { "Bad Request" };
            return errors.Any(json.Contains) ? new ServiceResponse<TResult>(ServiceStatus.Error, "Bad response received from server", json) : null;
        }
       
        private async Task<ServiceResponse<TResult>> SendRequest<TResult>(HttpClient client, HttpRequestMessage request, bool isServiceResponse)
        {
            Debug.WriteLine($"RestClient: Sending request '{client.BaseAddress} - {request.RequestUri}'");

            DateTime startTime = DateTime.UtcNow;
            HttpResponseMessage response = null;
            Exception exception = null;

            try
            {
#if DEBUG
                response = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
#else
                response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
#endif

                Debug.WriteLine($"RestClient: Success");

            }
            catch (WebException)
            {
                throw new NetworkException();
            }
            catch (Exception e)
            {
                Debug.WriteLine("RestClient error:" + e.Message);
                exception = e;
            }

            var responseInfo = new HttpResponseInfo
            {
                StartTime = startTime,
                Response = response,
                Exception = exception,
                Request = request,
            };

#if DEBUG
            return ProcessHttpResponse<TResult>(responseInfo, isServiceResponse).Result;
#else
            return await ProcessHttpResponse<TResult>(responseInfo, isServiceResponse);
# endif
        }

        public void SetCurrentAuthToken(AuthToken authToken)
        {
            CurrentAuthToken = authToken;
        }

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
        }
      
        #endregion Methods
    }
    
    internal struct HttpResponseInfo
    {
        #region Public Properties
        
        public Exception Exception { get; set; }
       
        public HttpRequestMessage Request { get; set; }
       
        public HttpResponseMessage Response { get; set; }
       
        public DateTime StartTime { get; set; }

        #endregion Public Properties
    }
}
