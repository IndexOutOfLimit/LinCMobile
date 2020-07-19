﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Views;

namespace LinC.ViewModels
{
    public class ReviewProductsPageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public List<Product> Products { get; set; }
        
        public CustomDelegateTimerCommand<Product> EditProductCommand { get; set; }
        public CustomDelegateTimerCommand<Product> DeleteProductCommand { get; set; }
        public CustomDelegateTimerCommand AddProductCommand { get; set; }
        public CustomDelegateTimerCommand SubmitButtonTappedCommand { get; }

        public ReviewProductsPageViewModel(ILinCApiServices services)
        {
            _services = services;

            EditProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await EditProductAction(item), (item) => true);
            DeleteProductCommand = new CustomDelegateTimerCommand<Product>(async (item) => await DeleteProductAction(item), (item) => true);
            AddProductCommand = new CustomDelegateTimerCommand(() => AddProductAction(), () => true);
            SubmitButtonTappedCommand = new CustomDelegateTimerCommand(async () => await SubmitProductsAction(), () => true);
        }

        public bool IsProductAvailable
        {
            get
            {
                return Products != null && Products.Count > 0;
            }           
        }

        public async Task DeleteProductAction(Product item)
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

        public async Task EditProductAction(Product item)
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

                await AppNavigationService.GoBackAsync();
            }
            );

            AppSpinner.HideLoading();
        }

        private async Task SubmitProductsAction()
        {
            if (Products == null || Products.Count == 0)
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("Submit Product", "No product available to submit.", "OK");
                return;
            }

            // Save to backend DB

            ThreadingHelpers.InvokeOnMainThread(async () =>
                await AppNavigationService.GoToAsync(nameof(UserDashboardPage).ToLower(),
                    (UserDashboardPageViewModel vm) =>
                    {
                        vm.UserDetails = App.UserDetails;
                        vm.Products = this.Products;
                    })
                );
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

                await AppNavigationService.GoBackAsync();
            }
            );
        }
    }
}
