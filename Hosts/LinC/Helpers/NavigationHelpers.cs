using System.Threading.Tasks;
using Xamarin.Forms;
using Cognizant.Hackathon.Mobile.Core.Helpers;

namespace LinC.Helpers
{
    public static class NavigationHelpers
    {
        // It provides a navigatable section for elements which aren't explicitly defined within the Shell.
        // For example, if one page is accessed from the fly-out through a MenuItem but it doesn't belong to any section
        public static ShellSection GetShellSection(Element element)
        {
            if (element == null)
                return null;

            var parent = element;
            var parentSection = parent as ShellSection;

            while (parentSection == null && parent != null)
            {
                parent = parent.Parent;
                parentSection = parent as ShellSection;
            }

            return parentSection;
        }

        public static async Task NavigateBackAsync() => ThreadingHelpers.InvokeOnMainThread(async () => await App.NavigationRoot.Navigation.PopAsync());

        public static async Task NavigateModallyBackAsync() => ThreadingHelpers.InvokeOnMainThread(async () => await App.NavigationRoot.Navigation.PopModalAsync());

        public static async Task NavigateToAsync(Page page)
        {
            ThreadingHelpers.InvokeOnMainThread(async () => await App.NavigationRoot.Navigation.PushAsync(page).ConfigureAwait(false));
        }

        internal static async Task NavigatePopToRootAsync()
        {
            var modalCount = App.NavigationRoot.Navigation.ModalStack.Count;
            for (int currModal = 0; currModal < modalCount; currModal++)
            {
                ThreadingHelpers.InvokeOnMainThread(async () => await App.NavigationRoot.Navigation.PopModalAsync(false));
            }
        }

        public static async Task NavigateModallyToAsync(Page page, bool animated = true)
        {
            ThreadingHelpers.InvokeOnMainThread(async () => await App.NavigationRoot.Navigation.PushModalAsync(page, animated).ConfigureAwait(false));
        }
    }
}