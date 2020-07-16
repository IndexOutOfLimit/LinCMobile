using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class MasterData
    {
        //[JsonProperty(PropertyName = "status")]
        //public string Status { get; set; }

        [JsonProperty(PropertyName = "usrTypeMasterList")]
        public List<UserType> UserTypeMaster { get; set; }

        [JsonProperty(PropertyName = "stateList")]
        public List<State> StateMaster { get; set; }

        [JsonProperty(PropertyName = "countryList")]
        public List<Country> CountryMaster { get; set; }

        [JsonProperty(PropertyName = "productTypes")]
        public List<ProductType> ProductTypeMaster { get; set; }

        //[JsonProperty(PropertyName = "productCategoryList")]
        //public List<ProductCategory> ProductCategoryList { get; set; }

        //[JsonProperty(PropertyName = "orgMasterList")]
        //public List<Organization> OrgMaster { get; set; }       

        //[JsonProperty(PropertyName = "storeMasterList")]
        //public List<Store> StoreMaster { get; set; }
    }

    public class UserType
    {
        [JsonProperty(PropertyName = "usrTypeMasterId")]
        public string UserTypeId { get; set; }

        [JsonProperty(PropertyName = "usrTypeName")]
        public string UserTypeName { get; set; }

        [JsonProperty(PropertyName = "usrTypeCode")]
        public string UserTypeCode { get; set; }

        public bool IsSelected { get; set; }
    }

    public class ProductCategory
    {
        [JsonProperty(PropertyName = "productCategoryId")]
        public string ProductCategoryId { get; set; }

        [JsonProperty(PropertyName = "productTypeId")]
        public string ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "productCategoryName")]
        public string ProductCategoryName { get; set; }

        [JsonProperty(PropertyName = "productCategoryCode")]
        public string ProductCategoryCode { get; set; }

        [JsonProperty(PropertyName = "dispaySeq")]
        public int DispaySeq { get; set; }
    }

    public class ProductType
    {
        [JsonProperty(PropertyName = "prdctTypeId")]
        public string ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "prdctTypeCode")]
        public string ProductTypeCode { get; set; }

        [JsonProperty(PropertyName = "prdctTypeName")]
        public string ProductTypeName { get; set; }        

        [JsonProperty(PropertyName = "prdctTypeDispSeq")]
        public int DisplaySeq { get; set; }
    }

    public class Organization
    {
        [JsonProperty(PropertyName = "orgId")]
        public int OrgId { get; set; }

        [JsonProperty(PropertyName = "orgTypeMasterId")]
        public int OrgTypeMasterId { get; set; }

        [JsonProperty(PropertyName = "orgName")]
        public string OrgName { get; set; }

        [JsonProperty(PropertyName = "orgCode")]
        public string OrgCode { get; set; }

        [JsonProperty(PropertyName = "orgDescription")]
        public string OrgDescription { get; set; }
    }
    
    public class Country
    {
        [JsonProperty(PropertyName = "countryId")]
        public string CountryId { get; set; }

        [JsonProperty(PropertyName = "countryName")]
        public string CountryName { get; set; }

        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "areaCode")]
        public string AreaCode { get; set; }
    }

    public class Store
    {
        [JsonProperty(PropertyName = "storeId")]
        public string StoreId { get; set; }

        [JsonProperty(PropertyName = "orgId")]
        public string OrgId { get; set; }

        [JsonProperty(PropertyName = "storeCode")]
        public string StoreCode { get; set; }

        [JsonProperty(PropertyName = "storeName")]
        public string StoreName { get; set; }
    }
}
