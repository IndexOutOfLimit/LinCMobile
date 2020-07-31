using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Views;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public List<Product> Products { get; set; }
        public bool ShouldModify { get; set; }

        public CustomDelegateTimerCommand<Product> EditProductCommand { get; set; }
        public CustomDelegateTimerCommand<Product> DeleteProductCommand { get; set; }
        public CustomDelegateTimerCommand AddProductCommand { get; set; }
        public CustomDelegateTimerCommand SubmitButtonTappedCommand { get; }
        public CustomDelegateTimerCommand<Product> ProductSelectionCommand { get; }
        public CustomDelegateTimerCommand<Product> IncreaseQuantityCommand { get; }
        public CustomDelegateTimerCommand<Product> DecreaseQuantityCommand { get; }

        public ProductsPageViewModel(ILinCApiServices services)
        {
            _services = services;

            EditProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await EditProductAction(item), (item) => true);
            DeleteProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await DeleteProductAction(item), (item) => true);
            AddProductCommand = new CustomDelegateTimerCommand(() => AddProductAction(), () => true);
            SubmitButtonTappedCommand = new CustomDelegateTimerCommand(async () => await SubmitProductsAction(), () => true);
            ProductSelectionCommand = new CustomDelegateTimerCommand<Product>((item) => SelectProductAction(item), (item) => true);
            IncreaseQuantityCommand = new CustomDelegateTimerCommand<Product>((item) => IncreaseQuantityAction(item), (item) => true);
            DecreaseQuantityCommand = new CustomDelegateTimerCommand<Product>((item) => DecreaseQuantityAction(item), (item) => true);
        }

        private void SelectProductAction(Product item)
        {
            item.IsSubmitted = !item.IsSubmitted;
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            if (UserDetails.UserTypeId.Equals(1))
            {
                ShouldModify = true;
            }
            else
            {
                ShouldModify = false;
            }
        }

        public async Task DeleteProductAction(Product item)
        {
            var response = await AppPopupInputService.ShowMessageOkCancelAlertPopup("Remove Product", "Are you sure to remove product?", "OK", "CANCEL");
            if(!response.Equals("ok",StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            ThreadingHelpers.InvokeOnMainThread(() =>
            {
                Products.Remove(item);
                var prds = Products.ToList();
                Products.Clear();
                Products = null;
                Products = prds;
            }
            );            
        }

        public async Task EditProductAction(Product item)
        {
            AppSpinner.ShowLoading();
            await Task.Delay(10);
            ThreadingHelpers.InvokeOnMainThread(async () =>
                {
                //    AppInitialiserService.SetInitialiser<AddProductPageViewModel>((AddProductPageViewModel vm) =>
                //    {
                //        vm.UserDetails = UserDetails;
                //        vm.ProductList = Products;
                //        vm.Product = item;
                //        vm.IsAddProduct = false;
                //    });

                    await AppNavigationService.GoBackAsync();
                }
            ) ;

            AppSpinner.HideLoading();
        }

        private async Task SubmitProductsAction()
        {
            if (Products == null || Products.Count == 0)
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("Submit Product", "No product available to submit.", "OK");
                return;
            }

            var selectedPrdCount = Products.Count(p => p.IsSubmitted);
            if (selectedPrdCount == 0)
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("Submit Product", "Please select one product to submit.", "OK");
                return;
            }

            // save to db
            try
            {
                AppSpinner.ShowLoading();

                var response = await _services.UserService.SaveOrders(Products, UserDetails);
                AppSpinner.HideLoading();

                if (response.Data != null)
                {
                    ThreadingHelpers.InvokeOnMainThread(async () =>
                         await AppNavigationService.GoToAsync(nameof(UserDashboardPage).ToLower(),
                            (UserDashboardPageViewModel vm) =>
                                {
                                    vm.UserDetails = App.UserDetails;
                                    vm.Products = Products;
                                    vm.Orders = response.Data;
                               })
                          );
                }
                else
                {
                    await AppPopupInputService.ShowMessageOkAlertPopup("Submit Product", "Unable to place order. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void AddProductAction()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                //    AppInitialiserService.SetInitialiser<AddProductPageViewModel>((AddProductPageViewModel vm) =>
                //    {
                //        vm.UserDetails = UserDetails;
                //        vm.ProductList = Products;
                //        vm.Product = new Product();
                //        vm.ProductTypes = App.MasterData.ProductTypeMaster;
                //        vm.IsAddProduct = true;
                //    });

                await AppNavigationService.GoBackAsync();
            });
        }

        private void IncreaseQuantityAction(Product item)
        {
            int qty = item.Quantity;
            qty += 1;
            
            ThreadingHelpers.InvokeOnMainThread(() =>
            {
                item.Quantity = qty;
                item.Price = item.UnitPrice * qty;
            });
        }
        private void DecreaseQuantityAction(Product item)
        {
            int qty = item.Quantity;

            qty -= 1;

            if (qty <= 0)
            {
                qty = 1;
            }
            ThreadingHelpers.InvokeOnMainThread(() =>
            {
                item.Quantity = qty;
                item.Price = item.UnitPrice * qty;
            });
        }
    }
}
