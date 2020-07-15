using System;
using Cognizant.Hackathon.Core.Common.Enum;
using Cognizant.Hackathon.Core.Common.Infrastructure;
using Cognizant.Hackathon.Core.Model.Enums;
using Cognizant.Hackathon.Core.Model.Interface;

namespace Cognizant.Hackathon.Core.Interface
{   
    public interface IClientState
    {
        Guid StateId { get; set; } 
        string ApiInfo { get; set; }       
        string AppConfiguration { get; set; }       
        DateTime CacheLastUpdated { get; set; }      
        string DeviceToken { get; set; }
        string DeviceInstallation { get; set; }
        DeviceType DeviceType { get; set; }
        IMember Member { get; set; }
        ObjectType DataUpdateTargetType { get; set; }
        string CurrentSearchPhrase { get; set; }
        void SetDeviceToken(string deviceToken, DeviceType deviceType);
        AppStatus AppStatus { get; set; }
        string CountryCode { get; set; }
        DateTime LastLogoutTime { get; set; }
        bool IsNotificationGranted { get; set; }        
        bool IsAuthenticated { get; }
    }
}