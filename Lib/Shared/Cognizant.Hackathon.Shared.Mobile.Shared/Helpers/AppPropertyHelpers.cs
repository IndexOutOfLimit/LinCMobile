using System.Threading.Tasks;
using Newtonsoft.Json;
//using Cognizant.Hackathon.Mobile.Core.Enums;
using Xamarin.Forms;
using Cognizant.Hackathon.Core.Common.Helpers;
using Xamarin.Essentials;
using System;
using Cognizant.Hackathon.Shared.Mobile.Core.Enums;

namespace Cognizant.Hackathon.Shared.Mobile.Shared.Helpers
{
    public static class AppPropertyHelpers
    {
        public static async Task<string> GetValueFromSecureStorage(string key)
        {
            try
            {
                return await SecureStorage.GetAsync(key) ?? string.Empty;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> SetValueToSecureStorage(string key, string securedValue)
        {
            try
            {
                await SecureStorage.SetAsync(key, securedValue);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> AddReplaceJsonToSecureStorage<T>(AppProperty key, T value)
        {            
            bool check = await ContainsSecureStorageKey(key.ToString());
            if (check)
                RemoveValueFromSecureStorage(key.ToString());

            string json;
            json = JsonConvert.SerializeObject(value);

            return await SetValueToSecureStorage(key.ToString(), json);           
        }

        public static bool RemoveValueFromSecureStorage(string key)
        {
            try
            {
                SecureStorage.Remove(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveAllValuesFromSecureStorage()
        {
            try
            {
                SecureStorage.RemoveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> ContainsSecureStorageKey(string key)
        {
            var returnValue = await GetValueFromSecureStorage(key);

            if(string.IsNullOrEmpty(returnValue))
            {
                return true;
            }

            return false;
        }

        public static bool ContainsKey(AppProperty key)
        {
            Application myApp = Application.Current;
            return myApp.Properties.ContainsKey(key.ToString());
        }

        public static async Task AddReplace<T>(AppProperty key, T value)
        {
            Application myApp = Application.Current;

            bool check = myApp.Properties.ContainsKey(key.ToString());
            if (check)
                myApp.Properties.Remove(key.ToString());

            myApp.Properties.Add(key.ToString(), value);

            await myApp.SavePropertiesAsync();
        }

        public static async Task AddReplaceValue<T>(AppProperty key, T value) where T : struct
        {
            Application myApp = Application.Current;

            bool check = myApp.Properties.ContainsKey(key.ToString());
            if (check)
                myApp.Properties.Remove(key.ToString());

            myApp.Properties.Add(key.ToString(), value);

            await myApp.SavePropertiesAsync();
        }

        public static async Task AddReplaceBinary<T>(AppProperty key, T value) where T : class
        {
            var bytes = SerialiserHelper.ToBytes(value);

            Application myApp = Application.Current;

            bool check = myApp.Properties.ContainsKey(key.ToString());
            if (check)
                myApp.Properties.Remove(key.ToString());

            myApp.Properties.Add(key.ToString(), bytes);

            await myApp.SavePropertiesAsync();
        }

        public static async Task AddReplaceJson<T>(AppProperty key, T value)
        {
            Application myApp = Application.Current;

            bool check = myApp.Properties.ContainsKey(key.ToString());
            if (check)
                myApp.Properties.Remove(key.ToString());

            string json;
            
            json = JsonConvert.SerializeObject(value);
           
            myApp.Properties.Add(key.ToString(), json);
            await myApp.SavePropertiesAsync();
        }

        public static T Get<T>(AppProperty key)
        {
            Application myApp = Application.Current;
            bool check = myApp.Properties.ContainsKey(key.ToString());

            if (check)
            {
                var property = myApp.Properties[key.ToString()];
                if (property is null)
                {
                    Clear(key);
                    return default(T);
                }

                return (T)property;
            }

            return default(T);
        }

        public static async Task<T> GetValue<T>(AppProperty key) where T : struct
        {
            Application myApp = Application.Current;
            bool check = myApp.Properties.ContainsKey(key.ToString());

            if (check)
            {
                var property = myApp.Properties[key.ToString()];
                return (T)property;
            }

            return default(T);
        }

        public static async Task<string> GetValue(AppProperty key)
        {
            Application myApp = Application.Current;
            bool check = myApp.Properties.ContainsKey(key.ToString());

            if (check)
            {
                var property = myApp.Properties[key.ToString()];
                return (string)property;
            }

            return default(string);
        }

        public static async Task<T> GetBinary<T>(AppProperty key) where T : class
        {
            Application myApp = Application.Current;
            bool check = myApp.Properties.ContainsKey(key.ToString());

            if (check)
            {
                var property = myApp.Properties[key.ToString()];
                if (property is null)
                {
                    await Clear(key);
                    return default(T);
                }

                var result = SerialiserHelper.FromBytes<T>((byte[])property);
                return result;
            }

            return default(T);
        }

        public static async Task<T> GetJson<T>(AppProperty key) where T : class
        {
            Application myApp = Application.Current;
            bool check = myApp.Properties.ContainsKey(key.ToString());

            if (check)
            {
                var property = myApp.Properties[key.ToString()] as string;
                if (string.IsNullOrEmpty(property))
                {
                    await Clear(key);
                    return default(T);
                }

                var result = JsonConvert.DeserializeObject<T>(property);
                return result;
            }

            return default(T);
        }

        public static async Task Clear()
        {
            Application myApp = Application.Current;

            myApp.Properties.Clear();
            await myApp.SavePropertiesAsync();
        }

        public static async Task Clear(AppProperty key)
        {
            Application myApp = Application.Current;
            bool check = myApp.Properties.ContainsKey(key.ToString());

            if (check)
            {
                myApp.Properties.Remove(key.ToString());
                await myApp.SavePropertiesAsync();
            }

        }

    }
}