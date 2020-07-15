using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using Cognizant.Hackathon.RestClient.Helpers;
using Cognizant.Hackathon.RestClient.Infrastructure;
using Cognizant.Hackathon.RestClient.Interfaces;

namespace Cognizant.Hackathon.RestClient.Models
{
    using Newtonsoft.Json;
    using System.IO;
     
    public class ServiceResponse<T> : IServiceResponse 
    {
        private string _errorMessage;

        #region Constructors and Destructor
              
        public ServiceResponse()
        {
            Errors = new ObservableCollection<string>();
            Errors.CollectionChanged += ErrorsOnCollectionChanged;
        }

        public ServiceResponse(ServiceStatus status, string message = "", string errorMessage = "", Exception exception = null, int amount = 0, int start = 0, T data = default(T), String stringData = null, ServiceErrorType errorType = ServiceErrorType.General, string errorCode = "") : this()
        {
            Status = status;
            Message = message;
            Amount = amount;
            Start = start;
            Data = data;
            StringData = stringData;
            ServiceErrorType = errorType;
            ServiceErrorCode = errorCode;

            if (exception != null)
            {
                Errors.Add(exception.Message);
                ErrorMessage = exception.Message;
                Message = exception.GetInnermostExceptionMessage();
                Status = ServiceStatus.Error;
            }
            else if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessage = errorMessage;
                Status = ServiceStatus.Error;
            }
        }

        public ServiceResponse(bool status, string message = "", string errorMessage = "", Exception exception = null, int amount = 0, int start = 0, T data = default(T))
            : this(ParseStatus(status), message, errorMessage, exception, amount, start, data)
        {
        }

        #endregion

        #region Public Properties
        
        public string Message { get; set; }

        public string ServiceErrorCode { get; set; }

        public ServiceStatus Status { get; set; }
       
        public T Data { get; set; }

        public string StringData { get; set; }

        public void SetData(object data)
        {
            Data = (T)data;
        }

        public HttpStatusCode? HttpStatus 
        {
            get
            {
                if (Enum.TryParse(StatusCode.ToString(), out HttpStatusCode code))
                    return code;
                else
                    return null;
            }
        }
       
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                    Status = ServiceStatus.Error;

                _errorMessage = value;
            }
        }

      
        public TimeSpan ElapsedTime { get; set; }
                
        public DateTime RequestDateTime { get; set; }
        
        public ObservableCollection<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets the record count.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the paging start.
        /// </summary>
        public int Start { get; set; }

        public ServiceErrorType ServiceErrorType { get; set; }

        public DateTime? ProcessingTimestamp { get; set; }

        public long? TotalCount { get; set; }

        // Hack to get image stream on
        public Stream Stream { get; set; }

        public byte[] Bytes { get; set; }

        public int StatusCode { get; set; }

        #endregion

        #region Methods
       
        public bool IsOK()
        {
            return Status == ServiceStatus.Success;
        }

        public bool IsStatusOK()
        {
            return StatusCode == 0 || StatusCode == 200;
        }
        
        private static ServiceStatus ParseStatus(bool status)
        {
            return status ? ServiceStatus.Success : ServiceStatus.Error;
        }

        private void ErrorsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if(notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add)
                Status = ServiceStatus.Error;
        }
        #endregion
               
        public override string ToString()
        {
            string json = JsonConvert.SerializeObject(this);
            if(string.IsNullOrEmpty(json))
                return base.ToString();

            return json;
        }
    }
}