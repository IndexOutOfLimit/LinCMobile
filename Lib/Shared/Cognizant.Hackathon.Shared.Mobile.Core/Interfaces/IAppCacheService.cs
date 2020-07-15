using System.Threading.Tasks;
using Cognizant.Hackathon.Shared.Mobile.Core.Enums;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Interfaces
{
    public interface IAppCacheService<TState> where TState : class
    {
        Task SaveToSecureStorage(string key, string securedValue);
        Task GetFromSecureStorage(string key);
        bool RemoveFromSecureStorage(string key);
        bool RemoveAllFromSecureStorage();
        Task SaveToSecureStorage();
        Task<T> GetValue<T>(AppProperty key) where T : struct;
        Task<string> GetValue(AppProperty key);

        Task Save();
        Task<bool> Restore();
        Task Save<T>(AppProperty key, T value) where T : class;
        Task<T> Get<T>(AppProperty key) where T : class;
        Task Clear();
        Task Clear(AppProperty key);
        TState State { get; }
    }
}