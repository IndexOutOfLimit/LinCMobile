using System;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class LandingPageViewModel: ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public CustomDelegateCommand RegisterButtonTappedCommand { get; }
        public CustomDelegateCommand SupplierCatButtonTappedCommand { get; }
        public CustomDelegateCommand ProductCatButtonTappedCommand { get; }
        public CustomDelegateCommand MapButtonTappedCommand { get; }
        public CustomDelegateCommand ChatButtonTappedCommand { get; }

        public LandingPageViewModel(ILinCApiServices services)
        {
            _services = services;

            RegisterButtonTappedCommand = new CustomDelegateCommand(async() =>  await RegisterAction(), () => true);
            SupplierCatButtonTappedCommand = new CustomDelegateCommand(() => SupplierCatAction(), () => true);
            ProductCatButtonTappedCommand = new CustomDelegateCommand(() => ProductCatAction(), () => true);
            MapButtonTappedCommand = new CustomDelegateCommand(() => MapAction(), () => true);
            ChatButtonTappedCommand = new CustomDelegateCommand(() => ChatAction(), () => true);
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);
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

            }
            finally
            {
                AppSpinner.HideLoading();
            }
        }

        private void ChatAction()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
                await AppNavigationService.GoToAsync(nameof(ChatPage).ToLower(),
                    (ChatPageViewModel vm) =>
                    {
                        vm.UserDetails = App.UserDetails;
                    })
                );
        }

        private void MapAction()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
                await AppNavigationService.GoToAsync(nameof(MapPage).ToLower(),
                    (MapPageViewModel vm) =>
                    {
                        vm.UserDetails = App.UserDetails;
                    })
                );
        }

        private void ProductCatAction()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
                 await AppNavigationService.GoToAsync(nameof(ProductCataloguePage).ToLower(),
                     (ProductCataloguePageViewModel vm) =>
                     {
                         vm.UserDetails = App.UserDetails;
                     })
                 );
        }

        private void SupplierCatAction()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
                 await AppNavigationService.GoToAsync(nameof(SupplierCataloguePage).ToLower(),
                     (SupplierCataloguePageViewModel vm) =>
                     {
                         vm.UserDetails = App.UserDetails;
                     })
                 );
        }

        private async Task RegisterAction()
        {
            //AppSpinner.ShowLoading();
            //await GetMasterData();
            //AppSpinner.HideLoading();

            ThreadingHelpers.InvokeOnMainThread(async () =>
                await AppNavigationService.GoToAsync(nameof(RegistrationPage).ToLower(),
                    (RegistrationPageViewModel vm) =>
                    {
                        //vm.UserDetails = new LinCUser();
                    })
                );
        }
    }
}
