using Cognizant.Hackathon.RestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cognizant.Hackathon.RestClient.Infrastructure
{
    /// <summary>
    ///     Handles OnDeleteObjectHasParentException errors
    /// </summary>
    public class NetworkException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkException"/> class.
        ///     Initializes a new instance of the <see cref="OnDeleteObjectHasParentException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner Exception.
        /// </param>
        public NetworkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NetworkException(string message)
            : base(message)
        {
        }

        public NetworkException()
            : base()
        {
        }

        #endregion
    }
        
    /// <summary>
    /// Handles javascript errors
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class ServiceResponseException<T> : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseException{T}"/> class.
        ///     Initializes a new instance of the <see cref="MDA.Common.ServiceResponseException"/> class.
        /// </summary>
        /// <param name="serviceResponse">
        /// The service Response.
        /// </param>
        public ServiceResponseException(ServiceResponse<T> serviceResponse)
            : base(serviceResponse.Message)
        {
            ServiceResponse = serviceResponse;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the service response.
        /// </summary>
        public ServiceResponse<T> ServiceResponse { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        #endregion
    }
}
