using System.Collections.Generic;
using Android.App;
using Android.OS;
using Cognizant.Hackathon.Shared.Mobile.Core.Helpers;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using Firebase.Analytics;

namespace LinC.Droid.Platforms
{
    public class AnalyticsService : IAnalyticsService
    {
        public void SetScreenAndClassName(string screenName, string screenClassName)
        {
            var context = CrossCurrentActivity.Current as Activity;
            var fireBaseAnalytics = FirebaseAnalytics.GetInstance(context);
            fireBaseAnalytics.SetCurrentScreen(context, screenName, screenClassName);
        }

        public void LogEvent(string eventId)
        {
            LogEvent(eventId, null);
        }

        public void LogEvent(string eventId, string paramName, string value = "")
        {
            LogEvent(eventId, new Dictionary<string, string>
        {
            {paramName, value}
        });
        }

        public void LogEvent(string eventId, IDictionary<string, string> parameters)
        {
            var fireBaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current as Activity);

            if (parameters == null)
            {
                fireBaseAnalytics.LogEvent(eventId, null);
                return;
            }

            var bundle = new Bundle();

            foreach (var item in parameters)
            {
                bundle.PutString(FirebaseAnalytics.Param.ItemId, item.Key);
                bundle.PutString(FirebaseAnalytics.Param.ItemName, item.Value);
            }

            fireBaseAnalytics.LogEvent(eventId, bundle);
        }

        public void SetUserId(string userId)
        {
            var fireBaseAnalytics = FirebaseAnalytics.GetInstance((CrossCurrentActivity.Current as Activity).ApplicationContext);

            fireBaseAnalytics.SetUserId(userId);
        }
    }
}
