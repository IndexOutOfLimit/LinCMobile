using System;
using Cognizant.Hackathon.Core.Common.Infrastructure;
using Cognizant.Hackathon.Core.Interface;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Interfaces;
using StandardAppConfig;

namespace Cognizant.Hackathon.Mobile.Shared.Services
{
    public class MobileAppLogger : ILogger
    {
        private readonly string _appName;
        private readonly string _appConfig;
        private readonly IAnalyticsService _analyticsService;

        public MobileAppLogger(IAnalyticsService analyticsService)
        {
            _appName = ConfigurationManager.AppSettings["app.name"];
            _appConfig = ConfigurationManager.AppSettings["app.config"];

            _analyticsService = analyticsService;
        }

        public void LogAction(Exception exception, Logger.LogType logType = Logger.LogType.ERROR, string message = null)
        {
            if (exception == null)
                exception = new Exception(message);
//#if __IOS__ || __ANDROID__
            try
            {
                switch (logType)
                {
                    case Logger.LogType.NAVIGATE:
                        break;
                    case Logger.LogType.INFO:
                        System.Diagnostics.Debug.WriteLine(message ?? exception?.Message, Logger.LogType.INFO.ToString());
                        _analyticsService.LogEvent(Logger.LogType.INFO.ToString(), "Info", message ?? exception?.Message);
                        break;
                    case Logger.LogType.WARNING:
                        System.Diagnostics.Debug.WriteLine(message ?? exception?.Message, Logger.LogType.WARNING.ToString());
                        _analyticsService.LogEvent(Logger.LogType.WARNING.ToString(), "Warning", message ?? exception?.Message);
                        break;
                    case Logger.LogType.ERROR:
                        System.Diagnostics.Debug.WriteLine(message ?? exception?.Message, Logger.LogType.ERROR.ToString());
                        _analyticsService.LogEvent(Logger.LogType.ERROR.ToString(), "Error", message ?? exception?.Message);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
//#else
            //;
//#endif
        }

        public void LogException(Exception exception)
        {
            LogAction(exception);
        }

        public void LogException(Exception exception, string message)
        {
            LogAction(exception);
        }

        public void LogInfo(string message)
        {
            //throw new NotImplementedException();
        }

        public void LogInfo(string eventId, string paramName, string value = "")
        {
            //System.Diagnostics.Debug.WriteLine(message, Logger.LogType.INFO.ToString());
            _analyticsService.LogEvent(eventId, paramName, value);
        }

        public void SetScreenAndClassName(string screenName, string screenClassName)
        {
            _analyticsService.SetScreenAndClassName(screenName, screenClassName);
        }

        public void SetUserId(string userId)
        {
            _analyticsService.SetUserId(userId);
        }
    }
}
