using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Views;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class SupplierCataloguePageViewModel: ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public List<ProductType> ProductTypes { get; set; }
        public ProductType SelectedProductType { get; set; }

        public List<LinCUser> Suppliers { get; set; }
        public bool IsSupplierSearched { get; set; }

        public CustomDelegateTimerCommand<LinCUser> SupplierSelectCommand { get; }
        public CustomDelegateTimerCommand<ProductType> PickerCellCommand { get; }

        public SupplierCataloguePageViewModel(ILinCApiServices services)
        {
            _services = services;

            PickerCellCommand = new CustomDelegateTimerCommand<ProductType>(async(item) => await PickerTapped(item), item => true);
            SupplierSelectCommand = new CustomDelegateTimerCommand<LinCUser>(async (item) => await SupplierTapped(item), item => true);

            IsSupplierSearched = false;
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            Suppliers = new List<LinCUser>();
            Suppliers.Add(UserDetails);
        }

        private async Task SupplierTapped(LinCUser item)
        {
            try
            {
                AppSpinner.ShowLoading();

                var response = await _services.UserService.GetUserProducts(UserDetails.UserId,
                                            item.UserId, SelectedProductType.ProductTypeId);

                if(response.Data.Item1 != null && response.Data.Item1.Count > 0)
                {

                    ThreadingHelpers.InvokeOnMainThread(async () =>
                      await AppNavigationService.GoToAsync(nameof(ProductsPage).ToLower(),
                          (ProductsPageViewModel vm) =>
                          {
                              vm.UserDetails = App.UserDetails;
                              vm.Products = response.Data.Item1;
                          })
                      );
                }

                AppSpinner.HideLoading();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task PickerTapped(ProductType item)
        {
            try
            {
                AppSpinner.ShowLoading();

                var response = await _services.UserService.GetSuppliers(UserDetails.UserId.Value, item.ProductTypeId, 4);

                if(response.Data.Item1 != null && response.Data.Item1.Count > 0)
                {
                    IsSupplierSearched = true;
                    Suppliers = response.Data.Item1;
                }
                else
                {
                    Suppliers = null;
                    IsSupplierSearched = false;
                }

                AppSpinner.HideLoading();
            }
            catch (Exception ex)
            {

            }
        }

        private async Task SearchAction()
        {
            throw new NotImplementedException();
        }
    }
}
