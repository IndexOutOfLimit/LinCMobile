using System;
using System.Collections.ObjectModel;
using System.Net;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Models;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    public static class ServiceResponseHelpers
    {
        /// <summary>
        /// Unhandled the service response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex">The ex.</param>
        public static ServiceResponse<T> UnhandledServiceResponse<T>(Exception ex)
        {
            return new ServiceResponse<T>(ServiceStatus.Error, "", ex.Message, ex)
            {
                ErrorMessage = ex.Message,
                StatusCode = 50001,
                ServiceErrorType = ServiceErrorType.Unhandled
            };
        }

        /// <summary>
        /// Add response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <param name="statusMessage">The status message.</param>
        /// <param name="data">The data.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="isError">if set to <c>true</c> [is error].</param>
        public static ServiceResponse<T> AddResponse<T>(ServiceResponse<T> response, string statusMessage, T data, int statusCode = 0, bool isError = true)
        {
            response.StatusCode = statusCode;

            if (isError)
            {
                response.Errors.Add(statusMessage);
                response.ErrorMessage = statusMessage;
            }
            else
                response.Message = statusMessage;

            response.Data = data;

            return response;
        }

        
        public static ServiceResponse<T> ErrorServiceResponse<T>(string errorMessage, string[] errors)
        {
            return new ServiceResponse<T>(ServiceStatus.Error)
            {
                ErrorMessage = errorMessage,
                Errors = new ObservableCollection<string>(errors),
                StatusCode = (int) HttpStatusCode.BadRequest,
            };
        }

        public static ServiceResponse<T> SuccessServiceResponse<T>(T data, string message)
        {
            return new ServiceResponse<T>(ServiceStatus.Success)
            {
                Message = message,
                StatusCode = (int) HttpStatusCode.OK,
                Data = data
            };
        }
    }
}