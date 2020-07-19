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
        public CustomDelegateTimerCommand RegisterCommand { get; }
        public CustomDelegateTimerCommand CallCommand { get; }

        public LoginPageViewModel(ILinCApiServices services)
        {
            _services = services;
            LoginCommand = new CustomDelegateTimerCommand(async () => await Login(), () => true);
            RegisterCommand = new CustomDelegateTimerCommand(() => Register(), () => true);
            CallCommand = new CustomDelegateTimerCommand(() => CallCustomerCare(), () => true);
        }

        public async Task GetMasterData()
        {
            try
            {
                AppSpinner.ShowLoading();
                var lookUpData = await _services.MasterDataService.GetMasterData();
                App.MasterData = lookUpData.Data;
            }
            catch (Exception ex)
            {
                App.MasterData = null;
            }
            finally
            {
                AppSpinner.HideLoading();
            }
        }

        private void ShowHideFlyout()
        {
            ThreadingHelpers.InvokeOnMainThread(() => ShowFlyoutCommand.Execute());
        }

        private void CallCustomerCare()
        {
            throw new NotImplementedException();
        }

        private void Register()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
                await AppNavigationService.GoToAsync(nameof(RegistrationPage).ToLower(),
                    (RegistrationPageViewModel vm) =>
                    {
                        //vm.UserDetails = new LinCUser();
                    })
                );
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
