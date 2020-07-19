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
using Plugin.Permissions.Abstractions;
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

        public List<UserType> UserTypeMaster { get; set; }
        public List<ProductType> ProductTypeMaster { get; set; }
        //public Organization DefaultOrganization { get; set; }

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
            UserDetails.UserTypeId = string.Empty;
            UserDetails.ProductTypeIds = string.Empty;
            IsOrgVisible = true;

            NextButtonTappedCommand = new CustomDelegateTimerCommand(async () => await NextButtonTapped(), () => true);            
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            //if(App.MasterData != null && App.MasterData.OrgMaster != null && App.MasterData.OrgMaster.Count > 0)
            if (App.MasterData != null)
            {
                //OrgMasterData = App.MasterData.OrgMaster;
                //DefaultOrganization = OrgMasterData[0];

                CountryMasterData = App.MasterData.CountryMaster;
                DefaultCountry = CountryMasterData[0];

                StateMasterData = App.MasterData.StateMaster;
                DefaultState = StateMasterData[0];

                UserTypeMaster = App.MasterData.UserTypeMaster;
                UserTypeMaster[0].IsSelected = true;
                SectionVisibilityAction(UserTypeMaster[0]);

                ProductTypeMaster = App.MasterData.ProductTypeMaster;
            }

            UseLocationSelectionAction(LinCLogicalType.True);
        }

        private void PickerTapped(object item)
        {
            string param = item as string;
            switch (param)
            {
                case "State":
                    if (DefaultState != null)
                    {
                        UserDetails.StateId = DefaultState.StateId;
                    }
                    break;
                case "Country":
                    if (DefaultCountry != null)
                    {
                        UserDetails.CountryId = DefaultCountry.CountryId;
                    }
                    break;
                case "Organization":
                    //if (DefaultOrganization != null)
                    //{
                    //    UserDetails.Organization = DefaultOrganization.OrgCode;
                    //}
                    break;
                default:
                    break;
            }
            
        }

        private void ServiceTypeSelectionAction(object selectionType)
        {
            try
            {
                //LinCServiceType type = (LinCServiceType)selectionType;
                ProductType type = (ProductType)selectionType;
                string itemToAdd = type.ProductTypeId.ToString();

                var lstServiceTypes = UserDetails.ProductTypeIds.Split(',').ToList();
                if(lstServiceTypes.Contains(itemToAdd))
                {
                    lstServiceTypes.Remove(type.ProductTypeId.ToString());
                }
                else
                {
                    lstServiceTypes.Add(type.ProductTypeId.ToString());
                }
                lstServiceTypes = lstServiceTypes.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                UserDetails.ProductTypeIds = string.Join(",", lstServiceTypes);

            }
            catch (Exception)
            {
                UserDetails.ProductTypeIds = string.Empty;
            }
        }

        private void UseRegistrationSelectionAction(object selectionType)
        {
            /*try
            {
                LinCUserRegisterType type = (LinCUserRegisterType)selectionType;
                UserDetails.RegisterType = type.ToString();
                IsOrgVisible = !UserDetails.RegisterType.Contains("Individual");
                UserDetails.Organization = "";
            }
            catch (Exception)
            {
                UserDetails.RegisterType = string.Empty;
            }*/
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
                UserType usrType = userType as UserType;
                
                UserDetails.UserTypeId = usrType.UserTypeId.ToString();
                switch (UserDetails.UserTypeId)
                {
                    case "2": //LinCUserType.Consumer:
                        IsOtherSectionVisible = false;
                        break;
                    case "1":// LinCUserType.Supplier:
                        IsOtherSectionVisible = true;
                        RegisterTypeText = "Supplier Type";
                        break;
                    case "3":// LinCUserType.Volunteer:
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
        private async Task<bool> ValidateFields()
        {
            if(string.IsNullOrWhiteSpace(UserDetails.UserName))
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please enter user name", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.UserSecret))
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please enter password", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.FirstName))
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please enter first name", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.Email))
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please enter email", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.Phone))
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please enter phone", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.AddressLine1) && !ShouldUseCurrentLocation)
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please enter address", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.Pin) && !ShouldUseCurrentLocation)
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please enter pin code", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.StateId) && !ShouldUseCurrentLocation)
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please select state", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserDetails.CountryId) && !ShouldUseCurrentLocation)
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please select country", "OK");
                return false;
            }

            if (!UserDetails.UserTypeId.Equals("2") && string.IsNullOrEmpty(UserDetails.ProductTypeIds))
            {
                await AppPopupInputService.ShowMessageOkAlertPopup("User Registration", "Please select one service type", "OK");
                return false;
            }

            return true;
        }

        private async Task NextButtonTapped()
        {
            try
            {
                //if(!await ValidateFields())
                //{
                //    return;
                //}

                //if (!IsOrgVisible)
                //{
                //    UserDetails.Organization = string.Empty;
                //}

                //switch (LinCAppUserType)
                //{
                //    case LinCUserType.Consumer:
                //        UserDetails.Organization = string.Empty;
                //        UserDetails.RegisterType = string.Empty;
                //        break;
                //    default:
                //        break;
                //}

                AppSpinner.ShowLoading();

                if (ShouldUseCurrentLocation)
                {
                    var status = await CheckPermission(Permission.Location);
                    if (status == PermissionStatus.Granted)
                    {
                        var location = await GetUserLocation();
                        if(location != null)
                        {
                            UserDetails.Latitude = location.Latitude.ToString();
                            UserDetails.Longitude = location.Longitude.ToString();
                            var placemark = await GetUserAddressFromLatLong(location.Latitude, location.Longitude);

                            if(placemark != null)
                            {
                                UserDetails.Pin = placemark.PostalCode;
                                UserDetails.AddressLine1 = $"{placemark.FeatureName}, {placemark.SubLocality}, {placemark.Locality}";
                                UserDetails.AddressLine2 = string.Empty;
                                UserDetails.CountryId = CountryMasterData.Where(l => l.CountryCode.Equals(placemark.CountryCode)).FirstOrDefault()?.CountryId;
                                UserDetails.StateId = StateMasterData.Where(l => l.StateName.Contains(placemark.AdminArea)).FirstOrDefault()?.StateId;
                            }                            
                        }                        
                    }
                }
                else
                {
                    string countryName = CountryMasterData.Where(l => l.CountryId.Equals(UserDetails.CountryId)).FirstOrDefault()?.CountryName;
                    string stateCode = StateMasterData.Where(l => l.StateId.Equals(UserDetails.StateId)).FirstOrDefault()?.StateCode;

                    string address = $"{UserDetails.AddressLine1} {UserDetails.AddressLine2} {stateCode} {countryName}";
                    // get location from address
                    var location = await GetUserLocationFromAddress(address);
                    if (location != null)
                    {
                        UserDetails.Latitude = location.Latitude.ToString();
                        UserDetails.Longitude = location.Longitude.ToString();

                        //var placemark = await GetUserAddressFromLatLong(location.Latitude, location.Longitude);

                    }
                }

                if(!UserDetails.UserTypeId.Equals("2"))
                {
                    if(!string.IsNullOrEmpty(UserDetails.ProductTypeIds))
                    {
                        UserDetails.ProductTypeIds = "[" + UserDetails.ProductTypeIds + "]";
                    }
                }
                else
                {
                    UserDetails.ProductTypeIds = string.Empty;
                }



                // Create User
                string userTypeId = UserDetails.UserTypeId; //remove later

                var response = await _services.UserService.CreateUserAsync(null, null, UserDetails);

                response.ServiceErrorCode = LinCTrasactionStatus.Success.ToString(); // remove later

                if (response.ServiceErrorCode.Equals(LinCTrasactionStatus.Success.ToString()))
                {
                    UserDetails = response.Data;

                    UserDetails = new LinCUser(); //remove later
                    UserDetails.UserTypeId = userTypeId;
                    UserDetails.UserId = "4"; //remove later
                    App.UserDetails = UserDetails;

                    // get product category list
                    var responsePrdCategory = await _services.MasterDataService.GetProductCategoryByUser(UserDetails.UserId);

                    if(responsePrdCategory.ServiceErrorCode.Equals(LinCTrasactionStatus.Success.ToString()))
                    {
                        App.MasterData.ProductCategoryList = responsePrdCategory.Data.ProductCategoryList;

                        ThreadingHelpers.InvokeOnMainThread(async () =>
                            await AppNavigationService.GoToAsync(nameof(AddProductPage).ToLower(),
                                (AddProductPageViewModel vm) =>
                                {
                                    vm.UserDetails = UserDetails;
                                    vm.IsAddProduct = true;
                                    vm.ProductTypes = App.MasterData.ProductTypeMaster;
                                })
                            );

                        //ThreadingHelpers.InvokeOnMainThread(async () =>
                        //    await AppNavigationService.GoToAsync(nameof(SupplierCataloguePage).ToLower(),
                        //        (SupplierCataloguePageViewModel vm) =>
                        //        {
                        //            vm.UserDetails = UserDetails;
                        //        })
                        //    );
                        //ThreadingHelpers.InvokeOnMainThread(async () =>
                        //    await AppNavigationService.GoToAsync(nameof(ProductCataloguePage).ToLower(),
                        //        (ProductCataloguePageViewModel vm) =>
                        //        {
                        //            vm.UserDetails = UserDetails;
                        //        })
                        //    );
                    }
                    else
                    {
                        AppErrorService.AddError(response);
                    }                    
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

        private string ValidateEntry()
        {
            return null;
        }        
    }
}
