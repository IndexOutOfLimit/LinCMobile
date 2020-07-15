using System;

namespace Cognizant.Hackathon.Mobile.Core.Exceptions
{
    public class HandledException : Exception
    {
        public bool IsLog { get; set; }
        public string Title { get; set; }
        public HandledException(string message, bool isLog = true, string title = null) : base(message)
        {
            IsLog = isLog;
            Title = title;
        }

        public HandledException(string message, Exception innerException, bool isLog = true, string title = null) : base(message, innerException)
        {
            IsLog = isLog;
            Title = title;
        }
    }
}
