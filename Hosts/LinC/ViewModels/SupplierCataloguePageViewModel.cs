using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class SupplierCataloguePageViewModel: ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public string SearchTextConsumer { get; set; }
        public string SearchTextStoreName { get; set; }
        public string SearchTextProductType { get; set; }
        public string SearchTextArea { get; set; }

        public List<LinCUser> Suppliers { get; set; }

        public CustomDelegateCommand SearchButtonTappedCommand { get; }
        public CustomDelegateCommand ResetButtonTappedCommand { get; }

        public SupplierCataloguePageViewModel(ILinCApiServices services)
        {
            _services = services;

            SearchButtonTappedCommand = new CustomDelegateCommand(async() => await SearchAction(), () => true);
            ResetButtonTappedCommand = new CustomDelegateCommand(() => ResetAction(), () => true);

        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            Suppliers = new List<LinCUser>();
            Suppliers.Add(UserDetails);
        }

        private void ResetAction()
        {
            SearchTextConsumer = string.Empty;
            SearchTextStoreName = string.Empty;
            SearchTextProductType = string.Empty;
            SearchTextArea = string.Empty;
        }

        private async Task SearchAction()
        {
            throw new NotImplementedException();
        }
    }
}
