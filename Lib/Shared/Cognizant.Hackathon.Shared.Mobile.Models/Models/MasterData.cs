using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class MasterData
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "productCategoryList")]
        public List<ProductCategory> ProductCategoryList { get; set; }

        [JsonProperty(PropertyName = "orgMasterList")]
        public List<Organization> OrgMaster { get; set; }

        [JsonProperty(PropertyName = "stateList")]
        public List<State> StateMaster { get; set; }

        [JsonProperty(PropertyName = "countryList")]
        public List<Country> CountryMaster { get; set; }

        [JsonProperty(PropertyName = "storeMasterList")]
        public List<Store> StoreMaster { get; set; }
    }    

    public class ProductCategory
    {
        [JsonProperty(PropertyName = "productCategoryId")]
        public int ProductCategoryId { get; set; }

        [JsonProperty(PropertyName = "productTypeId")]
        public int ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "productCategoryName")]
        public string ProductCategoryName { get; set; }

        [JsonProperty(PropertyName = "productCategoryCode")]
        public string ProductCategoryCode { get; set; }

        [JsonProperty(PropertyName = "dispaySeq")]
        public int DispaySeq { get; set; }
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
        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "countryName")]
        public string CountryName { get; set; }
    }

    public class Store
    {
        [JsonProperty(PropertyName = "storeId")]
        public int StoreId { get; set; }

        [JsonProperty(PropertyName = "orgId")]
        public int OrgId { get; set; }

        [JsonProperty(PropertyName = "storeCode")]
        public string StoreCode { get; set; }

        [JsonProperty(PropertyName = "storeName")]
        public string StoreName { get; set; }
    }
}
