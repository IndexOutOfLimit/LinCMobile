
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class ProductCataloguePageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public List<Product> Products { get; set; }

        public CustomDelegateCommand SearchButtonTappedCommand { get; }
        public CustomDelegateCommand ResetButtonTappedCommand { get; }

        public string SearchTextProductName { get; set; }

        public ProductCataloguePageViewModel(ILinCApiServices services)
        {
            _services = services;

            SearchButtonTappedCommand = new CustomDelegateCommand(async () => await SearchAction(), () => true);
            ResetButtonTappedCommand = new CustomDelegateCommand(() => ResetAction(), () => true);
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            Products = new List<Product>();
            Products.Add(new Product());
            Products.Add(new Product());
            Products.Add(new Product());
        }

        private void ResetAction()
        {
            SearchTextProductName = string.Empty;
        }

        private Task SearchAction()
        {
            throw new NotImplementedException();
        }
    }
}
