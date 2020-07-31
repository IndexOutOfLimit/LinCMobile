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
    public class CartPageViewModel: ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public List<Product> Orders { get; set; }
        public List<Product> Products { get; set; }
        public string OrdersHeadingText { get; set; }
        public string OrderSubmitText { get; set; }

        public bool IsConsumerUser { get; set; }

        public CustomDelegateTimerCommand<Product> EditProductCommand { get; set; }
        public CustomDelegateTimerCommand<Product> DeleteProductCommand { get; set; }
        public CustomDelegateTimerCommand AddProductCommand { get; set; }
        public CustomDelegateTimerCommand PlaceOrderCommand { get; set; }
        public CustomDelegateCommand<Product> ProductSelectionCommand { get; }
        public CustomDelegateCommand<Product> IncreaseQuantityCommand { get; }
        public CustomDelegateCommand<Product> DecreaseQuantityCommand { get; }

        public CartPageViewModel(ILinCApiServices services)
        {
            _services = services;

            EditProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await EditProductAction(item), (item) => true);
            DeleteProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await DeleteProductAction(item), (item) => true);
            AddProductCommand = new CustomDelegateTimerCommand(() => AddProductAction(), () => true);
            PlaceOrderCommand = new CustomDelegateTimerCommand(() => PlaceOrderAction(), () => true);
            ProductSelectionCommand = new CustomDelegateCommand<Product>((item) => ProductSelectionAction(item), (item) => true);
            IncreaseQuantityCommand = new CustomDelegateCommand<Product>((item) => IncreaseQuantityAction(item), (item) => true);
            DecreaseQuantityCommand = new CustomDelegateCommand<Product>((item) => DecreaseQuantityAction(item), (item) => true);

            OrdersHeadingText = "Orders";
            OrderSubmitText = "SUBMIT ORDER";
        }       

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);
           
            if (!IsOrderAvailable)
            {
                OrdersHeadingText = "No order has been placed.";
                OrderSubmitText = "PLACE ORDER";
            }

            if (UserDetails.UserTypeId.Equals("2"))
            {
                IsConsumerUser = true;
            }
        }

        public bool IsOrderAvailable
        {
            get
            {
                bool isAvailable = Orders != null && Orders.Count > 0;
                return isAvailable;
            }
        }

        private void DecreaseQuantityAction(Product item)
        {
            int qty = item.Quantity;

            qty -= 1;
            
            if (qty < 1)
            {
                qty = 1;
            }
            ThreadingHelpers.InvokeOnMainThread(() =>
            {
                item.Quantity = qty;
                item.Price = item.UnitPrice * qty;
            });
        }

        private void IncreaseQuantityAction(Product item)
        {
            int qty = item.Quantity;
                       
            qty += 1;

            if (qty >= 100)
            {
                qty = 100;
            }
            ThreadingHelpers.InvokeOnMainThread(() =>
            {
                item.Quantity = qty;
                item.Price = item.UnitPrice * qty;
            });
        }

        private void ProductSelectionAction(Product item)
        {
            item.IsSubmitted = !item.IsSubmitted;
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
                Orders.Remove(item);
                var orders = Orders.ToList();
                Orders.Clear();
                Orders = null;
                Orders = orders;

                if (IsOrderAvailable)
                {
                    OrderSubmitText = "SUBMIT ORDER";
                }
                else
                {
                    OrderSubmitText = "PLACE ORDER";
                    OrdersHeadingText = "No order has been placed.";                 
                }
            }
            );
        }

        private async Task EditProductAction(Product item)
        {
            AppSpinner.ShowLoading();
            await Task.Delay(10);
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                AppInitialiserService.SetInitialiser<ProductCataloguePageViewModel>((ProductCataloguePageViewModel vm) =>
                {
                    vm.UserDetails = UserDetails;
                    vm.Products = Products;
                   
                });
                await AppNavigationService.GoToAsync(nameof(ProductCataloguePage).ToLower());

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
                    vm.ProductList = Products;
                    vm.Product = new Product();
                    vm.ProductTypes = App.MasterData.ProductTypeMaster;
                    vm.IsAddProduct = true;
                });

                await AppNavigationService.GoToAsync(nameof(AddProductPage).ToLower());
            });
        }

        private void PlaceOrderAction()
        {
            if(IsOrderAvailable)
            {
                // Save to backend DB

                ThreadingHelpers.InvokeOnMainThread(async () =>
                   await AppNavigationService.GoToAsync(nameof(UserDashboardPage).ToLower(),
                       (UserDashboardPageViewModel vm) =>
                       {
                           vm.UserDetails = App.UserDetails;
                           vm.Products = this.Products;
                           //vm.Orders = Orders.Where(o => o.IsSubmitted).ToList();
                       })
                );                
            }
            else
            {
                ThreadingHelpers.InvokeOnMainThread(async () =>
                {
                    AppInitialiserService.SetInitialiser<ProductCataloguePageViewModel>((ProductCataloguePageViewModel vm) =>
                    {
                        vm.UserDetails = UserDetails;
                        vm.ProductTypes = App.MasterData.ProductTypeMaster;
                        //vm.ProductList = Products;
                        //vm.Product = new Product();
                        //vm.ProductTypes = App.MasterData.ProductTypeMaster;
                    });

                    await AppNavigationService.GoToAsync(nameof(ProductCataloguePage).ToLower());
                });
            }            
        }
    }
}
