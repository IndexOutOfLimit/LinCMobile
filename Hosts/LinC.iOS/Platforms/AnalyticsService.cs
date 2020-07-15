using System.Collections.Generic;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Firebase.Analytics;
using Foundation;

namespace LinC.iOS.Platforms
{
    public class AnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventId)
        {
            LogEvent(eventId, (IDictionary<string, string>)null);
        }

        public void LogEvent(string eventId, string paramName, string value)
        {
            LogEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void LogEvent(string eventId, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                foreach (var item in parameters)
                {
                    new Dictionary<object, object>().Add(item.Key, item.Value);
                }
                Analytics.LogEvent(eventId, new Dictionary<object, object>());
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary =
                NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);
        }

        public void SetUserId(string userId)
        {
            Analytics.SetUserId(userId);
        }
        public void SetScreenAndClassName(string screenName, string screenClassName)
        {
            Analytics.SetScreenNameAndClass(screenName, screenClassName);
        }
    }
}
