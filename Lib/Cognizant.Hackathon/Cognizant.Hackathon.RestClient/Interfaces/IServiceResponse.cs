using System;
using System.Collections.ObjectModel;
using System.Net;
using Cognizant.Hackathon.RestClient.Helpers;
using Cognizant.Hackathon.RestClient.Infrastructure;

namespace Cognizant.Hackathon.RestClient.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceResponse
    {
        /// <summary>
        /// The set data.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        void SetData(object data);

        /// <summary>
        /// Gets the error message.
        /// </summary>
        string ErrorMessage { get; set; }
        int Amount { get; set; }
        int Start { get; set; }
        long? TotalCount { get; set; }
        ServiceStatus Status { get; set; }
        TimeSpan ElapsedTime { get; set; }
        ObservableCollection<string> Errors { get; set; }
        ServiceErrorType ServiceErrorType { get; set; }
        DateTime? ProcessingTimestamp { get; set; }
        int StatusCode { get; set; }
        HttpStatusCode? HttpStatus { get; }
    }
}