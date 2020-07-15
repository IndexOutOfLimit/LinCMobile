using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Cognizant.Hackathon.Mobile.Core.Extensions
{
    public static class NavigationExtensions
    {
        public static async Task PushPopupAsyncSafe(this INavigation sender, PopupPage page, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test") return;
            
            await popupNavigation.PushAsync(page, animate);
        }

        public static async Task PopPopupAsyncSafe(this INavigation sender, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test") return;
            await popupNavigation.PopAsync(animate);
        }

        public static async Task PopAllPopupAsyncSafe(this INavigation sender, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test") return;
            await popupNavigation.PopAllAsync(animate);
        }

        public static async Task RemovePopupPageAsyncSafe(this INavigation sender, PopupPage page, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test") return;
            await popupNavigation.RemovePageAsync(page, animate);
        }
    }
}