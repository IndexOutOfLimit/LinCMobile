using System.Linq;
using Cognizant.Hackathon.Core.Interface;

namespace Cognizant.Hackathon.Core.Common.Extensions
{
    public static class ClientStateExtensions
    {
        public static void Set(this IClientState state, IClientState source)
        {
            foreach (var prop in source.GetType().GetProperties().Where(x => x.CanWrite))
                prop.SetValue(state, prop.GetValue(source));
        }
    }
}
