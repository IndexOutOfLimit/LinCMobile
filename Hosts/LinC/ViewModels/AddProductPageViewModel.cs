using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cognizant.Hackathon.Mobile.Core.Helpers;
using Cognizant.Hackathon.Mobile.Core.Infrastructure;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Enums;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Views;
using Xamarin.Forms;

namespace LinC.ViewModels
{
    public class AddProductPageViewModel : ViewModelBase
    {
        private readonly ILinCApiServices _services;
        LinCControl _defaultProductName;
        
        public List<ProductType> ProductTypes { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public Product Product { get; set; }
        public List<Product> ProductList { get; set; }
        public ProductType SelectedProductType { get; set; }
        public ProductCategory SelectedProductCategory { get; set; }

        public string NewProductName { get; set; }        
        public bool ProductNameEntryVisibility { get; set; }
        public bool IsAddProduct { get; set; }
        public string HeaderText { get; set; }

        public CustomDelegateTimerCommand<object> PickerCellCommand { get; }
        public CustomDelegateTimerCommand NextButtonTappedCommand { get; }
        public CustomDelegateTimerCommand AddNewProductNameCommand { get; }
        public CustomDelegateTimerCommand SaveNewProductNameCommand { get; }
        public CustomDelegateTimerCommand<string> AdjustQuantityCommand { get; }

        public AddProductPageViewModel(ILinCApiServices services)
        {
            _services = services;

            PickerCellCommand = new CustomDelegateTimerCommand<object>((item) =>  PickerTapped(item), item => true);
            NextButtonTappedCommand = new CustomDelegateTimerCommand(async () => await NextButtonTapped(), () => true);
            AddNewProductNameCommand = new CustomDelegateTimerCommand(() => AddNewProductNameAction(), () => true);
            SaveNewProductNameCommand = new CustomDelegateTimerCommand(() => SaveNewProductNameAction(), () => true);
            AdjustQuantityCommand = new CustomDelegateTimerCommand<string>((item) => AdjustQuantityAction(item), (item) => true);

            Product = new Product();
        }

        protected override async Task OnShellNavigated(string sender, ShellNavigatedEventArgs args)
        {
            await base.OnShellNavigated(sender, args);

            if(!IsAddProduct)
            {
                HeaderText = "Edit Product";

                SelectedProductType = ProductTypes.Where(l => l.ProductTypeId.Equals(Product.ProductTypeId)).FirstOrDefault();
                ProductCategories = App.MasterData.ProductCategoryList.Where(l => l.ProductTypeId.Equals(Product.ProductTypeId)).ToList();
                SelectedProductCategory = ProductCategories.Where(l => l.ProductCategoryId.Equals(Product.ProductCategoryId)).FirstOrDefault();
            }
            else
            {
                HeaderText = "Add Product";
                SelectedProductCategory = null;
                SelectedProductType = ProductTypes[0];
                //ProductCategories = null;
            }
            /*if(Product != null)
            {
                ProductTypes.DefaultItem = ProductTypes.ControlValues.Where(x => x.ItemId.Equals(Product.ProductTypeId)).FirstOrDefault();
                //if(!string.IsNullOrEmpty(Product.ProductNameId))
                //{
                //    DefaultProductName = ProductNames.Where(l => l.ParentId.Equals(Product.ProductTypeId)).FirstOrDefault();

                //}
                //DefaultProductName.SelectedIndex = 
            }
            else if(ProductTypes == null)
            {
                var productItems = InitializeProduct();
                ProductTypes = productItems.Item1;
                ProductNames = productItems.Item2;
                Product = productItems.Item3;
                DefaultProductName = productItems.Item2[0];                
            }*/

            //ProductCategories = App.MasterData.ProductCategoryList.Where(l => l.ProductTypeId.Equals(ProductTypes[0].ProductTypeId)).ToList();
        }

        public LinCControl DefaultProductName
        {
            get
            {
                return _defaultProductName;
            }
            set
            {
                if(value!= null)
                {
                    _defaultProductName = value;
                    RaisePropertyChanged("DefaultProductName");
                }
            }
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);

            ProductNameEntryVisibility = false;
        }

        private void AddNewProductNameAction()
        {
            ProductNameEntryVisibility = true;
        }

        private void AdjustQuantityAction(string item)
        {
            int qty = Product.Quantity;

            if (item.Equals("Minus"))
            {
                qty -= 1;
            }
            else
            {
                qty += 1;
            }

            if (qty < 0)
            {
                qty = 0;
            }
            ThreadingHelpers.InvokeOnMainThread(() =>
            {
                Product.Quantity = qty;
            });
        }

        private void SaveNewProductNameAction()
        {
            try
            {
                ThreadingHelpers.InvokeOnMainThread(() =>
                {                    
                    //LinCComonModel prdNameObj = new LinCComonModel() { Name = NewProductName, Value = NewProductName, ItemId = string.Empty };

                    //var controlValues = DefaultProductName.ControlValues.ToList();
                    //controlValues.Add(prdNameObj);

                    //DefaultProductName.ControlValues.Clear();
                    //DefaultProductName.ControlValues = null;

                    //DefaultProductName.ControlValues = controlValues;
                    //DefaultProductName.DefaultItem = prdNameObj;
                    //DefaultProductName.SelectedIndex = controlValues.Count - 1;                    

                    //Product.ProductNameId = string.Empty;
                    //Product.ProductName = prdNameObj.Name;
                    //Product.IsNewProductName = true;

                    NewProductName = string.Empty;
                    ProductNameEntryVisibility = false;
                });                
            }
            catch (Exception)
            {

            }            
        }

        private async Task NextButtonTapped()
        {
            //ThreadingHelpers.InvokeOnMainThread(async () =>
            //    await AppNavigationService.GoToAsync(nameof(ChatPage).ToLower(),
            //        (ChatPageViewModel vm) =>
            //        {

            //        })
            //    );

            if (ProductList == null)
            {
                ProductList = new List<Product>();
            }

            if(IsAddProduct)
            {
                ProductList.Add(Product);
            }
            
            ThreadingHelpers.InvokeOnMainThread(async () =>
                await AppNavigationService.GoToAsync(nameof(ReviewProductsPage).ToLower(),
                    (ReviewProductsPageViewModel vm) =>
                    {
                        vm.UserDetails = UserDetails;
                        vm.Products = ProductList;
                    })
                );
        }

        private void PickerTapped(object item)
        {
            try
            {
                #region previous code
                /*
                switch (item)
                {
                    case "ProductType":
                        if(ProductTypes != null)
                        {
                            var prdType = ProductTypes.DefaultItem;
                            DefaultProductName = ProductNames.Where(l => l.ParentId.Equals(prdType.ItemId)).FirstOrDefault();

                            Product.ProductTypeId = prdType.ItemId;
                            Product.ProductType = prdType.Name;

                            if(DefaultProductName?.ControlValues != null )
                            {
                                Product.ProductNameId = DefaultProductName.ControlValues[0].ItemId;
                                Product.ProductName = DefaultProductName.ControlValues[0].Name;
                            }
                        }                        
                        break;
                    case "ProductName":
                        if (DefaultProductName != null && DefaultProductName.SelectedIndex > -1)
                        {
                            Product.ProductNameId = DefaultProductName.ControlValues[DefaultProductName.SelectedIndex].ItemId;
                            Product.ProductName = DefaultProductName.ControlValues[DefaultProductName.SelectedIndex].Name;
                        }
                        break;
                    default:
                        break;
                }
                */

                #endregion

                if(item is ProductType)
                {
                    Product.ProductTypeId = ((ProductType)item).ProductTypeId;
                    Product.ProductType = ((ProductType)item).ProductTypeName;

                    ProductCategories = App.MasterData.ProductCategoryList.Where(l => l.ProductTypeId.Equals(Product.ProductTypeId)).ToList();

                }
                else if (item is ProductCategory)
                {
                    Product.ProductCategoryId = ((ProductCategory)item).ProductCategoryId;
                    Product.ProductCategory = ((ProductCategory)item).ProductCategoryName;
                }

                //Product.Quantity = 0;
                //Product.Price = 0;
                //Product.Description = string.Empty;

            }
            catch (Exception ex)
            {
            }            
        }

        private (LinCControl, List<LinCControl>, Product) InitializeProduct()
        {
            Product productObj = new Product();

            var productItems = GetProductTypes();

            productObj.ProductType = productItems.Item1.DefaultItem.Name;
            productObj.ProductTypeId = productItems.Item1.DefaultItem.ItemId;

            //productObj.ProductName = productItems.Item2?[0].DefaultItem.Name;
            //productObj.ProductNameId = productItems.Item2?[0].DefaultItem.ItemId;

            return (productItems.Item1, productItems.Item2, productObj);
        }

        private (LinCControl, List<LinCControl>) GetProductTypes()
        {
            var productTypes = new LinCControl();
            var productNames = new List<LinCControl>();

            List<LinCComonModel> productList = new List<LinCComonModel>();

            LinCComonModel prdItem = new LinCComonModel() { Name = "Grocery", Value = "Grocery", ItemId = "1" };
            productList.Add(prdItem);
            string[] prdNameArray = new string[] { "Rice", "Dal", "Soyabin" };
            productNames.Add(GetProductName(prdItem.ItemId, prdNameArray));

            prdItem = new LinCComonModel() { Name = "Medical", Value = "Medical", ItemId = "2" };
            productList.Add(prdItem);
            prdNameArray = new string[] { "Sanitizer", "Syringe", "Gauze" };
            productNames.Add(GetProductName(prdItem.ItemId, prdNameArray));

            prdItem = new LinCComonModel() { Name = "Cloth", Value = "Cloth", ItemId = "3" };
            productList.Add(prdItem);
            prdNameArray = new string[] { "Coat", "Trouser", "Skirt" };
            productNames.Add(GetProductName(prdItem.ItemId, prdNameArray));

            productTypes.ControlValues = productList;
            productTypes.SelectedIndex = 0;
            productTypes.Title = "Product Type";
            productTypes.DefaultItem = productList?[0];
            productTypes.SelectedIndex = 0;

            return (productTypes, productNames);
        }

        private LinCControl GetProductName(string parentId, string[] itemArray)
        {
            List<LinCComonModel> productNameList = new List<LinCComonModel>();

            if (itemArray.Length > 0)
            {
                int cnt = 0;

                foreach (var prdName in itemArray)
                {
                    LinCComonModel item = new LinCComonModel() { Name = prdName, Value = prdName, ItemId = (++cnt).ToString() };
                    productNameList.Add(item);
                }
            }

            var productName = new LinCControl()
            {
                ControlValues = productNameList,
                SelectedIndex = 0,
                Title = "Product Name",
                DefaultItem = productNameList?[0],
                ParentId = parentId

            };
            productName.SelectedIndex = 0;
            return productName;
        }
    }
}
