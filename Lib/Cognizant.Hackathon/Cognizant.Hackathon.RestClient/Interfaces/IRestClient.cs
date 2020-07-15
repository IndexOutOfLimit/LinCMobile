using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Models;
using Cognizant.Hackathon.RestClient.Providers;

namespace Cognizant.Hackathon.RestClient.Interfaces
{

    /// <summary>
    /// The RestClient interface.
    /// </summary>
    public interface IRestClient
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the status code.
        /// </summary>
        int StatusCode { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        IApiConfiguration Configuration { get; }

        AuthToken CurrentAuthToken { get; }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The execute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestVerb">The request verb.</param>
        /// <param name="action">The action.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="id">The id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="start">The start.</param>
        /// <param name="order">The order.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="paramMode">The parameter mode.</param>
        /// <param name="apiRoutePrefix">The API route prefix.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="authToken">The authentication token.</param>
        /// <param name="isServiceResponse">if set to <c>true</c> [is service response].</param>
        /// <param name="isAnonymous">if set to <c>true</c> [is anonymous].</param>
        /// <param name="skipHostCheck">if set to <c>true</c> [skip host check].</param>
        /// <param name="metaData"></param>
        /// <returns>
        /// The <see cref="ServiceResponse{T}" />.
        /// </returns>
        ServiceResponse<T> Execute<T>(
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
            Dictionary<MetaData, string> metaData = null);

        /// <summary>
        /// The execute.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="requestVerb">The request verb.</param>
        /// <param name="action">The action.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="id">The id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="start">The start.</param>
        /// <param name="order">The order.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="paramMode">The parameter mode.</param>
        /// <param name="apiRoutePrefix">The API route prefix.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="authToken">The authentication token.</param>
        /// <param name="isServiceResponse">if set to <c>true</c> [is service response].</param>
        /// <param name="isAnonymous">if set to <c>true</c> [is anonymous].</param>
        /// <param name="skipHostCheck">if set to <c>true</c> [skip host check].</param>
        /// <param name="metaData"></param>
        /// <returns>
        /// The <see cref="ServiceResponse" />.
        /// </returns>
        ServiceResponse<TResult> Execute<TResult, TBody>(HttpVerb requestVerb
            , [CallerMemberName] string action = null
            , TBody requestBody = default(TBody)
            , string id = null
            , int amount = -1
            , int start = -1
            , string order = ""
            , string[] includes = null
            , Dictionary<string, object> parameters = null
            , HttpParamMode paramMode = HttpParamMode.REST
            , string apiRoutePrefix = null
            , Dictionary<string, string> headers = null
            , AuthToken authToken = null
            , bool isServiceResponse = false
            , bool isAnonymous = false
            , bool skipHostCheck = false
            , Dictionary<MetaData, string> metaData = null);

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestVerb">The request verb.</param>
        /// <param name="action">The action.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="id">The id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="start">The start.</param>
        /// <param name="order">The order.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="paramMode">The parameter mode.</param>
        /// <param name="apiRoutePrefix">The API route prefix.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="isServiceResponse">if set to <c>true</c> [is service response].</param>
        /// <param name="isAnonymous">if set to <c>true</c> [is anonymous].</param>
        /// <param name="skipHostCheck">if set to <c>true</c> [skip host check].</param>
        /// <param name="metaData"></param>
        /// <returns>
        /// The <see cref="async Task" />.
        /// </returns>
        Task<ServiceResponse<T>> ExecuteAsync<T>(HttpVerb requestVerb
            , [CallerMemberName] string action = null
            , T requestBody = default(T)
            , string id = null
            , int amount = -1
            , int start = -1
            , string order = ""
            , string[] includes = null
            , Dictionary<string, object> parameters = null
            , HttpParamMode paramMode = HttpParamMode.REST
            , string apiRoutePrefix = null
            , Dictionary<string, string> headers = null
            , AuthToken authToken = null
            , bool isServiceResponse = false
            , bool isAnonymous = false
            , bool skipHostCheck = false
            , Dictionary<MetaData, string> metaData = null);

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TBody">The type of the body.</typeparam>
        /// <param name="requestVerb">The request verb.</param>
        /// <param name="action">The action.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="id">The id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="start">The start.</param>
        /// <param name="order">The order.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="paramMode">The parameter mode.</param>
        /// <param name="apiRoutePrefix">The API route prefix.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="authToken">The authentication token.</param>
        /// <param name="isServiceResponse">if set to <c>true</c> [is service response].</param>
        /// <param name="isAnonymous">if set to <c>true</c> [is anonymous].</param>
        /// <param name="skipHostCheck">if set to <c>true</c> [skip host check].</param>
        /// <param name="metaData"></param>
        /// <returns>
        /// The <see cref="async Task" />.
        /// </returns>
        Task<ServiceResponse<TResult>> ExecuteAsync<TResult, TBody>(HttpVerb requestVerb
            , [CallerMemberName] string action = null
            , TBody requestBody = default(TBody)
            , string id = null
            , int amount = -1
            , int start = -1
            , string order = ""
            , string[] includes = null
            , Dictionary<string, object> parameters = null
            , HttpParamMode paramMode = HttpParamMode.REST
            , string apiRoutePrefix = null
            , Dictionary<string, string> headers = null
            , AuthToken authToken = null
            , bool isServiceResponse = false
            , bool isAnonymous = false
            , bool skipHostCheck = false
            , Dictionary<MetaData, string> metaData = null);

        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="authToken">The authentication token.</param>
        /// <param name="headers">The headers.</param>
        Task<ServiceResponse<AuthToken>> GetAuthToken(string username, string password, AuthToken authToken, Dictionary<string, string> headers);

        /// <summary>
        /// Set the current AuthToken value
        /// </summary>
        /// <param name="authToken"></param>
        void SetCurrentAuthToken(AuthToken authToken);

        #endregion
    }
}