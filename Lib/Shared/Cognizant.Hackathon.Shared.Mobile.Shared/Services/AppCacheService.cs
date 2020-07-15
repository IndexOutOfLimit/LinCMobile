using System;
using System.Threading.Tasks;
using Cognizant.Hackathon.Shared.Mobile.Shared.Helpers;
using Cognizant.Hackathon.Mobile.Core.Exceptions;
using Cognizant.Hackathon.RestClient.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Core.Enums;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Services
{
    public class AppCacheService : IAppCacheService<ClientState>
    {
        private readonly IRestClient _restClient;
        private delegate void RestoreCurrentState();

        public AppCacheService(IRestClient restClient)
        {
            _restClient = restClient;

            new RestoreCurrentState(async () => await Restore()).Invoke();
        }
                
        public ClientState State { get; private set; }

        public async Task SaveToSecureStorage(string key, string securedValue)
        {
            await AppPropertyHelpers.SetValueToSecureStorage(key, securedValue);
        }

        public async Task GetFromSecureStorage(string key)
        {
            await AppPropertyHelpers.GetValueFromSecureStorage(key);
        }

        public bool RemoveFromSecureStorage(string key)
        {
            return AppPropertyHelpers.RemoveValueFromSecureStorage(key);
        }

        public bool RemoveAllFromSecureStorage()
        {
            return AppPropertyHelpers.RemoveAllValuesFromSecureStorage();
        }

        public async Task SaveToSecureStorage()
        {
            await AppPropertyHelpers.AddReplaceJsonToSecureStorage(AppProperty.State, State);
        }

        public async Task Save()
        {
            await AppPropertyHelpers.AddReplaceJson(AppProperty.State, State);
        }

        public async Task<bool> Restore()
        {
            try
            {
                State = await AppPropertyHelpers.GetJson<ClientState>(AppProperty.State) ?? new ClientState();                
            }
            catch (Exception ex)
            {
                throw new HandledException(ex.Message, false);
            }

            return true;
        }

        public async Task Save<T>(AppProperty key, T value) where T : class
        {
            await AppPropertyHelpers.AddReplaceJson<T>(key, value);
        }

        public async Task Clear()
        {
            _restClient.SetCurrentAuthToken(null);

            State = new ClientState();
            await AppPropertyHelpers.Clear();
        }

        public async Task Clear(AppProperty key)
        {
            await AppPropertyHelpers.Clear(key);
        }

        public async Task<T> Get<T>(AppProperty key) where T : class
        {
            return await AppPropertyHelpers.GetJson<T>(key);
        }

        public async Task<T> GetValue<T>(AppProperty key) where T : struct
        {
            return await AppPropertyHelpers.GetValue<T>(key);
        }

        public async Task<string> GetValue(AppProperty key)
        {
            return await AppPropertyHelpers.GetValue(key);
        }
    }
}
