using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Interfaces;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using LinC.Views;
using Xamarin.Forms;
using LinC.Helpers;
using FFImageLoading.Svg.Forms;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Xamarin.Essentials;
using LinC.Platforms;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;

namespace LinC.ViewModels
{
    public class LoginPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly ILinCApiServices _services;
        public CustomDelegateTimerCommand LoginCommand { get; }

        public LoginPageViewModel(ILinCApiServices services)
        {
            _services = services;
            LoginCommand = new CustomDelegateTimerCommand(async () => await Login(), () => true);
        }

        private void ShowHideFlyout()
        {
            ThreadingHelpers.InvokeOnMainThread(() => ShowFlyoutCommand.Execute());
        }

        private async Task Login()
        {
            try
            {
                AppSpinner.ShowLoading();               

                AppSpinner.HideLoading();
                AppErrorService.ProcessErrors();

            }
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
            finally
            {
                LoginCommand.ResetTimer();
            }
        }        
    }
}
