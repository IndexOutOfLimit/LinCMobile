using System;
using Cognizant.Hackathon.Shared.Mobile.Helpers.AppSettings.Interfaces;

namespace Cognizant.Hackathon.Shared.Mobile.Helpers.AppSettings
{
    public class EnvironmentAppSettingsHelper : IAppSettingsHelper
    {
        public string GetValue(string keyName)
        {
            return Environment.GetEnvironmentVariable(keyName, EnvironmentVariableTarget.Process);
        }
    }
}
