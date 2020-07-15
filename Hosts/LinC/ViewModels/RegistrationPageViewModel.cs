using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Enums;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Views;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;

        public bool IsCommonSectionVisible { get; set; }
        public bool IsOtherSectionVisible { get; set; }
        public string RegisterTypeText { get; set; }
        public bool IsOrgVisible { get; set; }
        public bool ShouldUseCurrentLocation { get; set; }

        public List<Organization> OrgMasterData { get; set; }
        public Organization DefaultOrganization { get; set; }
        public List<Country> CountryMasterData { get; set; }
        public Country DefaultCountry { get; set; }
        public List<State> StateMasterData { get; set; }
        public State DefaultState { get; set; }

        public CustomDelegateCommand<object> UserTypeSelectionCommand { get; }
        public CustomDelegateCommand<object> UseLocationSelectionCommand { get; }
        public CustomDelegateCommand<object> UserRegistrationSelectionCommand { get; }
        public CustomDelegateCommand<object> ServiceTypeSelectionCommand { get; }
        public CustomDelegateTimerCommand<object> PickerCellCommand { get; }

        public CustomDelegateTimerCommand NextButtonTappedCommand { get; }

        public RegistrationPageViewModel(ILinCApiServices services)
        {
            _services = services;
            UserTypeSelectionCommand = new CustomDelegateCommand<object>(userType => SectionVisibilityAction(userType), (userType) => true);
            UseLocationSelectionCommand = new CustomDelegateCommand<object>(selectionType => UseLocationSelectionAction(selectionType), (selectionType) => true);
            UserRegistrationSelectionCommand = new CustomDelegateCommand<object>(selectionType => UseRegistrationSelectionAction(selectionType), (selectionType) => true);
            ServiceTypeSelectionCommand = new CustomDelegateCommand<object>(selectionType => ServiceTypeSelectionAction(selectionType), (selectionType) => true);
            PickerCellCommand = new CustomDelegateTimerCommand<object>((item) => PickerTapped(item), item => true);

            UserDetails = new LinCUser();
            UserDetails.UserType = string.Empty;
            UserDetails.ServiceType = string.Empty;
            IsOrgVisible = true;

            NextButtonTappedCommand = new CustomDelegateTimerCommand(async () => await NextButtonTapped(), () => true);
            SectionVisibilityAction(LinCUserType.Supplier);
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            if(App.MasterData != null && App.MasterData.OrgMaster != null && App.MasterData.OrgMaster.Count > 0)
            {
                OrgMasterData = App.MasterData.OrgMaster;
                DefaultOrganization = OrgMasterData[0];

                CountryMasterData = App.MasterData.CountryMaster;
                DefaultCountry = CountryMasterData[0];

                StateMasterData = App.MasterData.StateMaster;
                DefaultState = StateMasterData[0];
            }
        }

        private void PickerTapped(object item)
        {
            string param = item as string;
            switch (param)
            {
                case "State":
                    if (DefaultState != null)
                    {
                        UserDetails.State = DefaultState.StateCode;
                    }
                    break;
                case "Country":
                    if (DefaultCountry != null)
                    {
                        UserDetails.Country = DefaultCountry.CountryCode;
                    }
                    break;
                case "Organization":
                    if (DefaultOrganization != null)
                    {
                        UserDetails.Organization = DefaultOrganization.OrgCode;
                    }
                    break;
                default:
                    break;
            }
            
        }

        private void ServiceTypeSelectionAction(object selectionType)
        {
            try
            {
                LinCServiceType type = (LinCServiceType)selectionType;
                string itemToAdd = type.ToString() + "|"; ;

                var arrServiceTypes = UserDetails.ServiceType.Split('|');
                arrServiceTypes = arrServiceTypes.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (arrServiceTypes.Length == 0)
                {
                    UserDetails.ServiceType += itemToAdd;
                }
                else
                {
                    if(!arrServiceTypes.Contains(type.ToString()))
                    {
                        UserDetails.ServiceType += itemToAdd;
                    }
                    else
                    {
                        UserDetails.ServiceType = UserDetails.ServiceType.Replace(itemToAdd, string.Empty);
                    }
                }
            }
            catch (Exception)
            {
                UserDetails.ServiceType = string.Empty;
            }
        }

        private void UseRegistrationSelectionAction(object selectionType)
        {
            try
            {
                LinCUserRegisterType type = (LinCUserRegisterType)selectionType;
                UserDetails.RegisterType = type.ToString();
                IsOrgVisible = !UserDetails.RegisterType.Contains("Individual");
                UserDetails.Organization = "";
            }
            catch (Exception)
            {
                UserDetails.RegisterType = string.Empty;
            }
        }

        private void UseLocationSelectionAction(object selectionType)
        {
            try
            {
                LinCLogicalType type = (LinCLogicalType)selectionType;
                UserDetails.UseCurrentLocation = bool.Parse(type.ToString());
                ShouldUseCurrentLocation = UserDetails.UseCurrentLocation;
            }
            catch (Exception)
            {
                UserDetails.UseCurrentLocation = false;
                ShouldUseCurrentLocation = false;
            }
        }

        private void SectionVisibilityAction(object userType)
        {
            try
            {
                LinCUserType type = (LinCUserType)userType;
                LinCAppUserType = type;
                UserDetails.UserType = type.ToString();
                switch (type)
                {
                    case LinCUserType.Consumer:
                        IsOtherSectionVisible = false;
                        break;
                    case LinCUserType.Supplier:
                        IsOtherSectionVisible = true;
                        RegisterTypeText = "Supplier Type";
                        break;
                    case LinCUserType.Volunteer:
                        IsOtherSectionVisible = true;
                        RegisterTypeText = "Volunteer Type";
                        break;
                    default:
                        break;
                }
                IsCommonSectionVisible = true;
            }
            catch (Exception ex)
            {

            }
           
        }

        private async Task NextButtonTapped()
        {
            try
            {
                if (!IsOrgVisible)
                {
                    UserDetails.Organization = string.Empty;
                }

                switch (LinCAppUserType)
                {
                    case LinCUserType.Consumer:
                        UserDetails.Organization = string.Empty;
                        UserDetails.RegisterType = string.Empty;
                        break;
                    default:
                        break;
                }

                AppSpinner.ShowLoading();

                if (ShouldUseCurrentLocation)
                {
                    var location = await GetUserLocation();
                }
                

                if(UserDetails.UseCurrentLocation)
                {
                    var userLocation = await Xamarin.Essentials.Geolocation.GetLocationAsync();

                    if(userLocation != null)
                    {
                        UserDetails.Latitude = userLocation.Latitude.ToString();
                        UserDetails.Longitude = userLocation.Longitude.ToString();
                    }
                }

                App.UserDetails = UserDetails;

                // Create User

                //ThreadingHelpers.InvokeOnMainThread(async () =>
                //    await AppNavigationService.GoToAsync(nameof(AddProductPage).ToLower(),
                //        (AddProductPageViewModel vm) =>
                //        {
                //            vm.UserDetails = UserDetails;
                //            vm.IsAddProduct = true;
                //        })
                //    );

                //ThreadingHelpers.InvokeOnMainThread(async () =>
                //    await AppNavigationService.GoToAsync(nameof(SupplierCataloguePage).ToLower(),
                //        (SupplierCataloguePageViewModel vm) =>
                //        {
                //            vm.UserDetails = UserDetails;
                //        })
                //    );
                ThreadingHelpers.InvokeOnMainThread(async () =>
                    await AppNavigationService.GoToAsync(nameof(ProductCataloguePage).ToLower(),
                        (ProductCataloguePageViewModel vm) =>
                        {
                            vm.UserDetails = UserDetails;
                        })
                    );

                AppErrorService.ProcessErrors();  
                AppSpinner.HideLoading();

            }
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
        }        

        private string ValidateEntry()
        {
            return null;
        }        
    }
}
