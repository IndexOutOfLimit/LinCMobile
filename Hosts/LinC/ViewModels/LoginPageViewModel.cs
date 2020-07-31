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
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using System.Linq;
using System.Collections.Generic;

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

            UserDetails = new LinCUser();
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
            ThreadingHelpers.InvokeOnMainThread(async () =>
                 await AppNavigationService.GoToAsync(nameof(ChatPage).ToLower(),
                     (ChatPageViewModel vm) =>
                     {
                         vm.UserDetails = App.UserDetails;
                     })
                 );
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
               //if(UserDetails.Email)
                AppSpinner.ShowLoading();
                var response = await _services.UserService.GetUserAsync(null, null, null, UserDetails.UserName, UserDetails.UserSecret);

                //Get Products & orders
                
                if (response.Data.Item1 != null)
                {
                    App.UserDetails = response.Data.Item1;
                    
                    int ? supplierId = null;
                    List<Product> prdList = new List<Product>();

                    switch (App.UserDetails.UserTypeId)
                    {
                        case 1: // SUPPLIER
                            var responseProdCat = await _services.UserService.GetProductCategoryByUser(App.UserDetails.UserId.Value);
                            App.MasterData.ProductCategoryList = responseProdCat.Data.ProductCategoryList;

                            var productTypes = responseProdCat.Data.ProductCategoryList.Select(l => l.ProductTypeId).Distinct();

                            supplierId = App.UserDetails.UserId.Value;
                            foreach (var item in productTypes)
                            {
                                var products = await _services.UserService.GetUserProducts(App.UserDetails.UserId.Value,
                                                       supplierId, item);

                                if (products.Data.Item1 != null && products.Data.Item1.Count > 0)
                                {
                                    prdList.AddRange(products.Data.Item1);
                                }
                            }
                            break;
                        case 2: //CONSUMER
                            break;
                        case 3: // VOLUNTEER
                            break;
                        default:
                            break;
                    }

                    // get orders for the user
                    var orders = _services.UserService.GetOrders(App.UserDetails);

                    ThreadingHelpers.InvokeOnMainThread(async () =>
                        await AppNavigationService.GoToAsync(nameof(UserDashboardPage).ToLower(),
                           (UserDashboardPageViewModel vm) =>
                           {
                               vm.UserDetails = App.UserDetails;
                               vm.Products = prdList;
                               vm.Orders = orders.Result.Data;
                           })
                         );
                }
                else
                {
                    AppErrorService.AddError(response);
                }

                AppSpinner.HideLoading();
                AppErrorService.ProcessErrors();

            }
            catch (Exception ex)
            {
                HandleUIError(ex);
            }            
        }        
    }
}
