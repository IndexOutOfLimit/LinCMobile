using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Views;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class UserDashboardPageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public List<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public string OrdersHeadingText { get; set; }
        public string ProductsHeadingText { get; set; }
        public bool IsConsumerUser { get; set; }

        public CustomDelegateTimerCommand<Product> EditProductCommand { get; set; }
        public CustomDelegateTimerCommand<Product> DeleteProductCommand { get; set; }
        public CustomDelegateTimerCommand AddProductCommand { get; set; }
        public CustomDelegateTimerCommand PlaceOrderCommand { get; set; }
        public CustomDelegateCommand<Product> ProductSelectionCommand { get; }

        public UserDashboardPageViewModel(ILinCApiServices services)
        {
            _services = services;

            EditProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await EditProductAction(item), (item) => true);
            DeleteProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await DeleteProductAction(item), (item) => true);
            AddProductCommand = new CustomDelegateTimerCommand(() => AddProductAction(), () => true);
            PlaceOrderCommand = new CustomDelegateTimerCommand(() => PlaceOrderAction(), () => true);
            ProductSelectionCommand = new CustomDelegateCommand<Product>((item) => ProductSelectionAction(item), (item) => true);

            ProductsHeadingText = "Your products";
            OrdersHeadingText = "Orders";
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            if (!IsProductAvailable)
            {
                ProductsHeadingText = "No product available. Please add products.";
            }

            if (!IsOrderAvailable)
            {
                OrdersHeadingText = "No order has been placed.";
            }

            if(UserDetails.UserTypeId.Equals(2))
            {
                IsConsumerUser = true;
            }
        }

        public bool IsProductAvailable
        {
            get
            {
                return Products != null && Products.Count > 0;
            }
        }

        public bool IsOrderAvailable
        {
            get
            {
                return Orders != null && Orders.Count > 0;
            }
        }

        private void ProductSelectionAction(Product item)
        {
            item.ShouldAddToCart = !item.ShouldAddToCart;
        }

        private async Task DeleteProductAction(Product item)
        {
            var response = await AppPopupInputService.ShowMessageOkCancelAlertPopup("Remove Product", "Are you sure to remove product?", "OK", "CANCEL");
            if (!response.Equals("ok", StringComparison.InvariantCultureIgnoreCase))
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

        private async Task EditProductAction(Product item)
        {
            AppSpinner.ShowLoading();
            await Task.Delay(10);
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                AppInitialiserService.SetInitialiser<AddProductPageViewModel>((AddProductPageViewModel vm) =>
                {
                    vm.UserDetails = UserDetails;
                    vm.ProductList = Products;
                    vm.Product = item;
                    vm.IsAddProduct = false;
                });
                await AppNavigationService.GoToAsync(nameof(AddProductPage).ToLower());

                //await AppNavigationService.GoBackAsync();
            }
            );

            AppSpinner.HideLoading();
        }

        private void AddProductAction()
        {
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                AppInitialiserService.SetInitialiser<AddProductPageViewModel>((AddProductPageViewModel vm) =>
                {
                    vm.UserDetails = UserDetails;
                    vm.ProductList = null;// Products;
                    vm.Product = new Product();
                    vm.ProductTypes = App.MasterData.ProductTypeMaster;
                    vm.ProductCategories = App.MasterData.ProductCategoryList;
                    vm.IsAddProduct = true;
                    vm.Product.Quantity = 1;
                });

                await AppNavigationService.GoToAsync(nameof(AddProductPage).ToLower());
            });
        }

        private void PlaceOrderAction()
        {
            try
            {
                /*ThreadingHelpers.InvokeOnMainThread(async () =>
                {
                    AppInitialiserService.SetInitialiser<ProductCataloguePageViewModel>((ProductCataloguePageViewModel vm) =>
                    {
                        vm.UserDetails = UserDetails;
                        vm.ProductTypes = App.MasterData.ProductTypeMaster;                       
                    });

                    await AppNavigationService.GoToAsync(nameof(ProductCataloguePage).ToLower());
                });*/


                ThreadingHelpers.InvokeOnMainThread(async () =>
                    await AppNavigationService.GoToAsync(nameof(SupplierCataloguePage).ToLower(),
                        (SupplierCataloguePageViewModel vm) =>
                        {
                            vm.UserDetails = UserDetails;
                            vm.ProductTypes = App.MasterData.ProductTypeMaster;
                        })
                    );
            }
            catch (Exception ex)
            {

            }
            
        }

    }
}
