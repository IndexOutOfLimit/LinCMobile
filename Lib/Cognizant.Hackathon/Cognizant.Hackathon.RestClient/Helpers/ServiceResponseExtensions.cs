using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.RestClient.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    public static class ServiceResponseExtensions
    {
        /// <summary>
        /// The as service response.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <typeparam name="TReturnType">
        /// </typeparam>
        /// <returns>
        /// The <see cref="ServiceResponse{T}"/>.
        /// </returns>
        public static ServiceResponse<TReturnType> AsServiceResponse<TReturnType>(this TReturnType data, int start = 0, int amount = 0, DateTime? processingTimestamp = null, long totalCount = 0, string message = null, string errorMessage = null)
        {
            amount = amount != 0 
                ? amount
                : data == null 
                  ? 0 
                  : data?.GetType().GetInterface(nameof(ICollection)) != null 
                    ? ((ICollection)data).Count 
                    : 1;

            return new ServiceResponse<TReturnType>
            {
                Data = data,
                Start = start,
                Amount = amount,
                RequestDateTime = DateTime.UtcNow,
                ProcessingTimestamp = processingTimestamp,
                TotalCount = totalCount,
                Message = message,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// The as service response.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <typeparam name="TReturnType">
        /// </typeparam>
        /// <returns>
        /// The <see cref="ServiceResponse"/>.
        /// </returns>
        public static ServiceResponse<TReturnType> ToServiceResponse<TOriginalType, TReturnType>(
            this ServiceResponse<TOriginalType> originalServiceResponse, string message = "")
        {
            return new ServiceResponse<TReturnType>
            {
                RequestDateTime = DateTime.UtcNow,
                Status = originalServiceResponse.Status,
                ErrorMessage = originalServiceResponse.ErrorMessage,
                Message = string.IsNullOrEmpty(message) ? originalServiceResponse.Message : message,
                ElapsedTime = originalServiceResponse.ElapsedTime,
                Errors = originalServiceResponse.Errors,
                ServiceErrorType = originalServiceResponse.ServiceErrorType,
                StatusCode = originalServiceResponse.StatusCode
            };
        }

        /// <summary>
        /// The get data.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T GetData<T>(this ServiceResponse<T> response, T defaultValue = default(T))
        {
            if (response == null)
            {
                LogServiceException("Null response received from server");
                return defaultValue;
            }

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                string message = string.IsNullOrEmpty(response.Message) ? "Error" : response.Message;
                LogServiceException($"error for type {typeof(T).Name} : {message} - {response.ErrorMessage}");
                return defaultValue;
            }

            return response.Data;
        }

        /// <summary>
        /// The log service exception.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        private static void LogServiceException(Exception exception)
        {
            // TODO: Log the exception
            throw exception;
        }

        /// <summary>
        /// The log service exception.
        /// </summary>
        /// <param name="exceptionMessage">
        /// The exception message.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        private static void LogServiceException(string exceptionMessage)
        {
            // TODO: Log the exception
            throw new Exception(exceptionMessage);
        }

        /// <summary>
        /// The cast list.
        /// </summary>
        /// <param name="listType">
        /// The cast type.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public static IServiceResponse CastListAsServiceResponse(Type listType, IEnumerable list)
        {
            var itemType = listType.GenericTypeArguments.First();
            var typedList = CastList(itemType, list);

            var serviceResponselistType = typeof(ServiceResponse<>).MakeGenericType(listType);

            var response = (IServiceResponse)Activator.CreateInstance(serviceResponselistType);

            response.SetData(typedList);
            response.Amount = typedList.Count;

            return response;
        }

        /// <summary>
        /// The cast list.
        /// </summary>
        /// <param name="itemType">
        /// The cast type.
        /// </param>
        /// <param name="enumerable">
        /// The enumerable.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public static IList CastList(Type itemType, IEnumerable enumerable)
        {
            Type listType = typeof(List<>).MakeGenericType(itemType);
            var list = (IList)Activator.CreateInstance(listType);
            foreach (object item in enumerable)
            {
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Unhandled the service response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex">The ex.</param>
        public static ServiceResponse<T> UnhandledServiceResponse<T>(this Exception ex)
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
        public static ServiceResponse<T> AddResponse<T>(this ServiceResponse<T> response, string statusMessage, T data, int statusCode = 0, bool isError = true)
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
    }
}
