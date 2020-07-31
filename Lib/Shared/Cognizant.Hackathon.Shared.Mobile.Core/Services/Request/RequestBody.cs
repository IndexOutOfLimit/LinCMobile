using System;
using System.Collections.Generic;
using Cognizant.Hackathon.Shared.Mobile.Core.Services.Response;
using Cognizant.Hackathon.Shared.Mobile.Models;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Services.Request
{   
    public class UserReqBody
    {
        public LinCUser UserInfo;
    }

    public class UserLoginReqBody
    {
        [JsonProperty(PropertyName = "usrName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "usrPass")]
        public string UserSecret { get; set; }
    }

    public class SuppliersReqBody
    {
        [JsonProperty(PropertyName = "usrId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "productTypeMasterId")]
        public int ProductTypeMasterId { get; set; }

        [JsonProperty(PropertyName = "searchWithin")]
        public int SearchWithin { get; set; }

    }

    public class GetProductsReqBody
    {
        [JsonProperty(PropertyName = "usrId")]
        public int? UsrId { get; set; }

        [JsonProperty(PropertyName = "supplierId")]
        public int? SupplierId { get; set; }

        [JsonProperty(PropertyName = "productTypeMasterId")]
        public int ProductTypeMasterId { get; set; }
    }

    public class ProdCategoryReq
    {
        [JsonProperty(PropertyName = "usrId")]
        public int? UserId { get; set; }
    }

    public class AddProductRequest
    {
        [JsonProperty(PropertyName = "addNewProduct")]
        public List<ProductReq> Products { get; set; }

        [JsonProperty(PropertyName = "usrId")]
        public int UserId { get; set; }
    }

    public class ProductReq
    {
        [JsonProperty(PropertyName = "prdctCatId")]
        public int ProductCatId { get; set; }

        [JsonProperty(PropertyName = "prdctName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "prdctDesc")]
        public string ProductDesc { get; set; }

        [JsonProperty(PropertyName = "prdctRate")]
        public double ProductRate { get; set; }

        [JsonProperty(PropertyName = "prdctQty")]
        public int ProductQty { get; set; }
    }

    public class SaveOrderReq
    {
        [JsonProperty(PropertyName = "orderId")]
        public int? OrderId { get; set; }

        [JsonProperty(PropertyName = "orderStatus")]
        public string OrderStatus { get; set; }

        [JsonProperty(PropertyName = "consumerId")]
        public int? ConsumerId { get; set; }

        [JsonProperty(PropertyName = "consumer")]
        public string Consumer { get; set; }

        [JsonProperty(PropertyName = "supplierId")]
        public int? SupplierId { get; set; }

        [JsonProperty(PropertyName = "supplier")]
        public string Supplier { get; set; }

        [JsonProperty(PropertyName = "isSelfPickup")]
        public short IsSelfPickup { get; set; }

        [JsonProperty(PropertyName = "isVolunteered")]
        public short IsVolunteered { get; set; }

        [JsonProperty(PropertyName = "volunteerId")]
        public int? VolunteerId { get; set; }

        [JsonProperty(PropertyName = "volunteer")]
        public string Volunteer { get; set; }

        [JsonProperty(PropertyName = "orderTotal")]
        public int? OrderTotal { get; set; }

        [JsonProperty(PropertyName = "products")]
        public List<OrderProductReq> Products { get; set; }
    }

    public class OrderProductReq
    {
        [JsonProperty(PropertyName = "orderDetailId")]
        public int? OrderDetailId { get; set; }

        [JsonProperty(PropertyName = "productId")]
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "usrProductInventoryTrxId")]
        public int UserProductInventoryTrxId { get; set; }

        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "productDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty(PropertyName = "quantityOrdered")]
        public int QuantityOrdered { get; set; }

        [JsonProperty(PropertyName = "productRate")]
        public double ProductRate { get; set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public double TotalPrice { get; set; }
    }

    public class GetOrderReq
    {
        [JsonProperty(PropertyName = "consumerId")]
        public int? ConsumerId { get; set; }

        [JsonProperty(PropertyName = "supplierId")]
        public int? SupplierId { get; set; }

        [JsonProperty(PropertyName = "volunteerId")]
        public int? VolunteerId { get; set; }

        [JsonProperty(PropertyName = "orderId")]
        public int? OrderId { get; set; }

        [JsonProperty(PropertyName = "searchType")]
        public string SearchType { get; set; } //CONSUMER  SUPPLIER VOLUNTEER
    }

    public class DocumentMailDataReq
    {
        public string OperationName { get; set; }
    }

    public class EmailInput
    {
        public string UserCode { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DefaultSub { get; set; }
        public string DocumentName { get; set; }
        public string Email { get; set; }
    }

    public class MailReqBody
    {
        public string OperationName { get; set; }
    }

    public class MailInputDetails
    {
        [JsonProperty]
        public List<CommonDictionary> Fields { get; set; }
        public string PropertyAddress { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string DefaultSub { get; set; }
        public string Subject { get; set; }
    }
}
