using System.Collections.Generic;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
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
        //orderDetailId 
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

        [JsonProperty(PropertyName = "quantityOrdered")]
        public int Q { get; set; }

    }

    public class ProductTypeResponse
    {
        [JsonProperty(PropertyName = "productTypeMasterId")]
        public int ProductTypeMasterId { get; set; }

        [JsonProperty(PropertyName = "productTypeName")]
        public string ProductTypeName { get; set; }
    }

    public class SupplierResponse
    {
        [JsonProperty(PropertyName = "usrId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "fisrtName")]
        public string FisrtName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phoneNum")]
        public string PhoneNumber { get; set; }

        [JsonConverter(typeof(JsonExponentialConverter))]
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonConverter(typeof(JsonExponentialConverter))]
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "addr1")]
        public string Addr1 { get; set; }

        [JsonProperty(PropertyName = "addr2")]
        public string Addr2 { get; set; }

        [JsonProperty(PropertyName = "pin")]
        public int Pin { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "fullAddress")]
        public string FullAddress { get; set; }

        [JsonConverter(typeof(JsonExponentialConverter))]
        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "productTypeList")]
        public List<ProductTypeResponse> ProductTypes { get; set; }
    }

    public class SuppliersResponse
    {
        [JsonProperty(PropertyName = "supplierList")]
        public List<SupplierResponse> Suppliers { get; set; }
    }
}
