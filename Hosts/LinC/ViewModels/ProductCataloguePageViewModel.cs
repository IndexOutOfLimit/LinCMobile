
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
    public class ProductCataloguePageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public List<Product> Products { get; set; }
        public List<ProductType> ProductTypes { get; set; }
        public ProductType SelectedProductType { get; set; }

        public CustomDelegateCommand SearchButtonTappedCommand { get; }
        public CustomDelegateCommand ResetButtonTappedCommand { get; }
        //public CustomDelegateTimerCommand<ProductType> PickerCellCommand { get; }
        public CustomDelegateCommand AddToCartCommand { get; }
        public CustomDelegateCommand<Product> ProductSelectionCommand { get; }

        public string SearchTextProductName { get; set; }
        public bool IsSupplierSearched { get; set; }

        public ProductCataloguePageViewModel(ILinCApiServices services)
        {
            _services = services;

            SearchButtonTappedCommand = new CustomDelegateCommand(async () => await SearchAction(), () => true);
            
            //PickerCellCommand = new CustomDelegateTimerCommand<ProductType>((item) => PickerTapped(item), item => true);
            AddToCartCommand = new CustomDelegateCommand(async () => await AddToCartAction(), () => true);
            ProductSelectionCommand = new CustomDelegateCommand<Product>((item) => ProductSelectionAction(item), (item) => true);
            IsSupplierSearched = false;
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            //Products = new List<Product>();
            //Products.Add(new Product());
            //Products.Add(new Product());
            //Products.Add(new Product());
        }


        private void ProductSelectionAction(Product item)
        {
            item.ShouldAddToCart = !item.ShouldAddToCart;
        }

        private void ResetAction()
        {
            SearchTextProductName = string.Empty;
        }

        private Task SearchAction()
        {
            throw new NotImplementedException();
        }

        private async Task AddToCartAction()
        {
            try
            {
                ThreadingHelpers.InvokeOnMainThread(async () =>
                await AppNavigationService.GoToAsync(nameof(CartPage).ToLower(),
                    (CartPageViewModel vm) =>
                    {
                        vm.UserDetails = App.UserDetails;
                        vm.Products = this.Products;
                        vm.Orders = this.Products.Where(p => p.ShouldAddToCart).ToList();
                    })
                );
            }
            catch (Exception ex)
            {

            }
        }

        private void PickerTapped(object item)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
