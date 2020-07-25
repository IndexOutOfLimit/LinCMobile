using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Response
{
    public class ResponseMessage<T> where T : class
    {
        [JsonProperty]
        public T Body { get; set; }

        [JsonProperty]
        public T Error { get; set; }
    }

    public class ProductCategoryResponse
    {
        [JsonProperty(PropertyName = "productCategoryMasterId")]
        public int ProductCategoryMasterId { get; set; }

        [JsonProperty(PropertyName = "productCategoryName")]
        public string ProductCategoryName { get; set; }

        [JsonProperty(PropertyName = "productCategoryCode")]
        public string ProductCategoryCode { get; set; }

        [JsonProperty(PropertyName = "products")]
        public List<ProductResponse> Products { get; set; }

    }

    public class ProductsResponse
    {
        public List<ProductCategoryResponse> productCategoryList { get; set; }

    }

    public class ProductResponse
    {
        [JsonProperty(PropertyName = "productId")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "usrProductInventoryTrxId")]
        public int UsrProductInventoryTrxId { get; set; }

        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "productDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty(PropertyName = "productRate")]
        public double ProductRate { get; set; }

        [JsonProperty(PropertyName = "availableQuantity")]
        public int AvailableQuantity { get; set; }

    }
}
