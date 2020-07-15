using System.Collections.Generic;
using System.Threading.Tasks;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.RestClient.Interfaces
{

    /// <summary>
    /// The RestClient interface.
    /// </summary>
    public interface ITokenClient
    {
        /// <summary>
        /// Gets the authentication token.
        /// </summary>
        /// <param name="requestFor">The request for.</param>
        /// <param name="extraRequest">The extra request.</param>
        /// <param name="headers">The headers.</param>
        Task<ServiceResponse<AuthToken>> GetAuthToken(string requestFor, Dictionary<string, string> extraRequest, Dictionary<string, string> headers);

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <param name="headers">The headers.</param>
        Task<ServiceResponse<AuthToken>> RefreshAuthToken(Dictionary<string, string> headers = null);

    }
}