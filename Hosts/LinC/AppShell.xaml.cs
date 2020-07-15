using System.Linq;
using LinC.ViewModels;
using Xamarin.Forms;

namespace LinC
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static string PreviousRoute { get; set; }

        public AppShell()
        {
            try
            {
                ViewModelLocator.RegisterRoutes();
                InitializeComponent();
            }
            catch (System.Exception ex)
            {

            }
            
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            // tracking the Flyout Closed or Opened status
            if (propertyName == nameof(FlyoutIsPresented))
            {
                // broadcasting the Flyout Closed status
                if (!FlyoutIsPresented)
                    MessagingCenter.Send<string, bool>(nameof(AppShell), nameof(Shell.FlyoutIsPresented), FlyoutIsPresented);
            }
        }
        public void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
        {
            var routeOut = e?.Current?.Location?.OriginalString?.Split('/')?.LastOrDefault()?.Replace("/", "");
            PreviousRoute = e?.Current?.Location?.OriginalString;

            MessagingCenter.Send<string, ShellNavigatingEventArgs>(nameof(AppShell), routeOut + $"{nameof(OnShellNavigating)}Out", e);

            string routeIn;
            if (e == null && e.Target == null)
                return;

            routeIn = e?.Target?.Location?.OriginalString?.Split('/')?.Last()?.Replace("/", "");
            MessagingCenter.Send<string, ShellNavigatingEventArgs>(nameof(AppShell), routeIn + $"{nameof(OnShellNavigating)}In", e);
        }

        public async void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
        {
            var route = e?.Current?.Location?.OriginalString?.Split('/')?.LastOrDefault()?.Replace("/", "");
            MessagingCenter.Send<string, ShellNavigatedEventArgs>(nameof(AppShell), route + nameof(OnShellNavigated), e);
        }       
    }
}
