using System;
using System.Reflection;
using Newtonsoft.Json;
using Cognizant.Hackathon.Core.Common.Enum;
using Cognizant.Hackathon.Core.Common.Json;
using Cognizant.Hackathon.Core.Model.Enums;
using Cognizant.Hackathon.Core.Model.Interface;
using System.Collections.Generic;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure
{
    public class ClientState : ILinCClientState
    {
        public ClientState()
        {
        }
        public Guid StateId { get; set; }
        public AppStatus AppStatus { get; set; } = AppStatus.Running;
        public bool DeviceIsRegistered { get; set; }
        public bool IsAuthenticated => false;
        public bool IsNotificationGranted { get; set; }
        public bool IsPushNotificationSubscribed { get; set; }
        public bool IsRememberMyMobileNumber { get; set; }
        public DateTime CacheLastUpdated { get; set; }
        public DateTime LastLogoutTime { get; set; }
        public DeviceType DeviceType { get; set; }
        public Guid DeviceId { get; set; }

        [JsonConverter(typeof(ConcreteConverter<LinCUser>))]
        public IMember Member { get; set; } = new LinCUser();

        public ObjectType DataUpdateTargetType { get; set; }
        public string ApiInfo { get; set; }
        public string AppConfiguration { get; set; }
        public string CountryCode { get; set; }
        public string CurrentSearchPhrase { get; set; }
        public string DeviceInstallation { get; set; }
        public string DeviceToken { get; set; }

        public void SetDeviceToken(string deviceToken, DeviceType deviceType)
        {
            throw new NotImplementedException();
        }
    }
}
