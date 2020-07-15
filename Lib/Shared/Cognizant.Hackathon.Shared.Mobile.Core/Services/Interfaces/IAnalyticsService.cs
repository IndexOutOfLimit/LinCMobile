using System.Collections.Generic;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces
{
    public interface IAnalyticsService
    {
        void LogEvent(string eventId);
        void LogEvent(string eventId, string paramName, string value);
        void LogEvent(string eventId, IDictionary<string, string> parameters);
        void SetUserId(string userId);
        void SetScreenAndClassName(string screenName, string screenClassName);
    }
}
