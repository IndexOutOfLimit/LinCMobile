using System;
using System.Collections.Generic;
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

    }
}
