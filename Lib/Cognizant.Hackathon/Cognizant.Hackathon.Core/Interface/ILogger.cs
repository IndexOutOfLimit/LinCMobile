using System;
using Cognizant.Hackathon.Core.Common.Infrastructure;

namespace Cognizant.Hackathon.Core.Interface
{
    public interface ILogger
    {
        void LogException(Exception exception);

        void LogInfo(string message);
        void LogInfo(string eventId, string paramName, string value = "");
        void LogException(Exception exception, string message);

        void LogAction(Exception exception, Logger.LogType logType = Logger.LogType.ERROR, string message = null);

        void SetUserId(string userId);
        void SetScreenAndClassName(string screenName, string screenClassName);
    }
}