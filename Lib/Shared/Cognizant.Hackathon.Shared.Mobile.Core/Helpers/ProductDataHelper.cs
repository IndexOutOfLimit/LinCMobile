using System;
using System.Collections.Generic;
using System.Linq;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Helpers
{
    public class ProductDataHelper
    {

        public static List<Product> ConvertResponseProducts(ProductsResponse prodResponse)
        {
            List<Product> products = null;

            if(prodResponse.productCategoryList != null && prodResponse.productCategoryList.Count > 0)
            {
                foreach (var prodcategory in prodResponse.productCategoryList)
                {
                    if(prodcategory.Products != null && prodcategory.Products.Count > 0)
                    {
                        products = new List<Product>();
                        foreach (var item in prodcategory.Products)
                        {
                            var product = new Product();
                            product.ProductCategoryId = prodcategory.ProductCategoryMasterId;
                            product.ProductCategory = prodcategory.ProductCategoryName;
                            product.ProductId = item.ProductId;
                            product.ProductName = item.ProductName;
                            product.Price = item.ProductRate;
                            product.Description = item.ProductDescription;
                            product.UsrProductInventoryTrxId = item.UsrProductInventoryTrxId;
                            product.Quantity = item.AvailableQuantity;
                            //........

                            products.Add(product);
                        }
                       
                    }
                }
            }

            return products;
        }

        public static List<LinCUser> ConvertSuppliersResponse(SuppliersResponse suppliersResponse)
        {
            List<LinCUser> suppliers = new List<LinCUser>();

            try
            {
                if (suppliersResponse != null && suppliersResponse.Suppliers.Count > 0)
                {
                    foreach (var item in suppliersResponse.Suppliers)
                    {
                        LinCUser supplier = new LinCUser();

                        supplier.UserId = item.UserId;
                        supplier.Email = item.Email;

                        supplier.FirstName = item.FisrtName;
                        supplier.FirstName = item.FisrtName;
                        supplier.LastName = item.LastName;
                        supplier.MiddleName = item.MiddleName;
                        supplier.Phone = item.PhoneNumber;
                        supplier.Pin = item.Pin;
                        supplier.Latitude = item.Latitude;
                        supplier.Longitude = item.Longitude;

                        supplier.AddressLine1 = item.Addr1;
                        supplier.AddressLine2 = item.Addr2;
                        supplier.FullAddress = item.FullAddress;
                        supplier.StateName = item.State;
                        supplier.CountryName = item.Country;
                        supplier.Distance = Math.Floor(item.Distance);

                        supplier.ProductTypeIds = item.ProductTypes.Select(l => l.ProductTypeMasterId).Distinct().ToList();

                        suppliers.Add(supplier);
                    }
                }
            }
            catch (Exception ex)
            {

            }
                        
            
            return suppliers;
        }
    }
}
