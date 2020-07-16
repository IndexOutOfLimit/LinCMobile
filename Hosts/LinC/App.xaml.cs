using System.Threading.Tasks;
using Cognizant.Hackathon.Shared.Mobile.Bootstrap;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Helpers;
using LinC.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Application = Xamarin.Forms.Application;
using Device = Xamarin.Forms.Device;

namespace LinC
{
    public partial class App : Application
    {
        private static NavigableElement _navigationRoot;
        private static string _deviceDensity;

        public static string DeviceDensity
        {
            get { return _deviceDensity; }
            set
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    _deviceDensity = ParseAndroidDeviceDensity(value);
                }
                else
                {
                    _deviceDensity = value;
                }
            }
        }

        public static LinCUser UserDetails { get; set; }

        public static MasterData MasterData { get; set; }

        public static string DeviceType { get; set; }

        public static App Instance => Current as App;

        public static AppShell Shell => Instance.MainPage as AppShell;

        public static AppShellViewModel ShellViewModel => Shell.BindingContext as AppShellViewModel;

        public static NavigableElement NavigationRoot
        {
            get => NavigationHelpers.GetShellSection(_navigationRoot) ?? _navigationRoot;
            set => _navigationRoot = value;
        }

        private static string ParseAndroidDeviceDensity(string densityNumber)
        {
            string density = "";

            switch (densityNumber)
            {
                case "1.5":
                    density = "high";
                    break;
                case "2":
                case "2.6":
                case "3.5":
                    density = "xhigh";
                    break;
                default:
                    density = "xhigh";
                    break;

            }

            return density;
        }


        public App()
        {
            try
            {
                //InitAppCenter();/InitFirebase();

                InitializeComponent();

                MainPage = new AppShell();

                // Setting the keyboard popover behavior for Pages (Adjust Layout to be scrollable)
                if (Device.RuntimePlatform == Device.Android)
                {
                    Current?.On<Xamarin.Forms.PlatformConfiguration.Android>()
                        .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
                }
            }
            catch (System.Exception)
            {

            }

        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            try
            {
                _ = BootStrapper.Container?.Resolve<IMainViewModel>().OnStart();
            }
            catch (System.Exception ex)
            {
                ViewModelLocator.Init();
                _ = BootStrapper.Container?.Resolve<IMainViewModel>().OnStart();
            }            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            BootStrapper.Container.Resolve<IMainViewModel>().OnSleep();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            BootStrapper.Container.Resolve<IMainViewModel>().OnResume();
        }
    }
}
