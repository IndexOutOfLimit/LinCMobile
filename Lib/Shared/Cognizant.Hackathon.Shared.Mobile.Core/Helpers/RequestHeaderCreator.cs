using System;
using System.Collections.Generic;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Request;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Helpers
{
    public static class RequestHeaderCreator
    {
        private static string CompanyCode;
        private static string UserCode;
        private static string UserId;
        private static string DeviceType;
        private static string DeviceDensity;

        public static RequestHeader GetRequestHeader(string deviceType, string deviceDensity, string compCode = null, string userCode = null, string userId = null)
        {
            var header = new RequestHeader
            {
                UserCode = userCode,
                UserId = userId,
                AppVersion = Xamarin.Essentials.AppInfo.VersionString,
                DeviceType = deviceType,
                Manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer,
                Platform = Xamarin.Essentials.DeviceInfo.Platform.ToString(),
                Density = deviceDensity,
                Model = Xamarin.Essentials.DeviceInfo.Model,
                Timestamp = "",
                OSVersion = Xamarin.Essentials.DeviceInfo.VersionString

            };

            return header;
        }

        public static Dictionary<string, string> GetWebApiClientHeader()
        {
            var headers = new Dictionary<string, string>
            {
                { "APPVER", Xamarin.Essentials.AppInfo.VersionString },
                { "MDVER","1" },
                { "x-requested-with", "XMLHttpRequest" }
            };

            return headers;
        }
    }
}
