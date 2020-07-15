using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    public class RetryHelpers
    {
        /// <summary>
        /// retry an action
        /// </summary>
        /// <param name="action">what to retry</param>
        /// <param name="interval">interval in ms</param>
        /// <param name="condition">keep trying until this is met</param>
        /// <param name="retryCount">try x times</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public static bool Retry(Func<bool> action, int interval, int retryCount = 3)
        {
            bool result = false;
            var exceptions = new List<Exception>();
            var retryInterval = TimeSpan.FromMilliseconds(interval);

            for (var retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    result = action.Invoke();

                    if (!result)
                        Task.Delay(retryInterval).Wait();
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    if (exceptions.All(x => x.Message != ex.Message))
                        exceptions.Add(ex);

                    Task.Delay(retryInterval).Wait();
                }
            }

            if (!result && !exceptions.Any())
                return false;

            if (exceptions.Count() == 1) throw exceptions[0];

            if (exceptions.Count() > 1)
                throw new AggregateException(exceptions);

            return result;
        }

        public static async Task<bool> Retry(Func<Task<bool>> action, int interval, int retryCount = 3)
        {
            bool flag = false;
            List<Exception> source = new List<Exception>();
            TimeSpan delay = TimeSpan.FromMilliseconds((double)interval);
            for (int index = 0; index < retryCount; ++index)
            {
                try
                {
                    flag = await action();
                    if (flag)
                        return true;
                    Task.Delay(delay).Wait();
                }
                catch (Exception ex)
                {
                    if (source.All<Exception>((Func<Exception, bool>)(x => x.Message != ex.Message)))
                        source.Add(ex);
                    Task.Delay(delay).Wait();
                }
            }
            if (!flag && !source.Any<Exception>())
                return false;
            if (source.Count<Exception>() == 1)
                throw source[0];
            if (source.Count<Exception>() > 1)
                throw new AggregateException((IEnumerable<Exception>)source);
            return flag;
        }
    }
}
