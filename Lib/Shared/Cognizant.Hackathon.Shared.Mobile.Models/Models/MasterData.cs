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

        [JsonProperty(PropertyName = "productTypeMasterList")]
        public List<ProductType> ProductTypeMaster { get; set; }

        [JsonProperty(PropertyName = "productCategories")]
        public List<ProductCategory> ProductCategoryList { get; set; }

        //[JsonProperty(PropertyName = "orgMasterList")]
        //public List<Organization> OrgMaster { get; set; }       

        //[JsonProperty(PropertyName = "storeMasterList")]
        //public List<Store> StoreMaster { get; set; }
    }

    public class UserType
    {
        [JsonProperty(PropertyName = "usrTypeMasterId")]
        public int UserTypeId { get; set; }

        [JsonProperty(PropertyName = "usrTypeName")]
        public string UserTypeName { get; set; }

        [JsonProperty(PropertyName = "usrTypeCode")]
        public string UserTypeCode { get; set; }

        public bool IsSelected { get; set; }
    }

    public class ProductCategory
    {
        [JsonProperty(PropertyName = "prdctTypeId")]
        public int ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "prdctTypeCode")]
        public string ProductTypeCode { get; set; }

        [JsonProperty(PropertyName = "prdctTypeName")]
        public string ProductTypeName { get; set; }

        [JsonProperty(PropertyName = "prdctTypeDispSeq")]
        public int ProducyTypeDisplaySeq { get; set; }

        [JsonProperty(PropertyName = "prdctCatId")]
        public int ProductCategoryId { get; set; }

        [JsonProperty(PropertyName = "prdctCatName")]
        public string ProductCategoryName { get; set; }

        [JsonProperty(PropertyName = "prdctCatCode")]
        public string ProductCategoryCode { get; set; }

        [JsonProperty(PropertyName = "prdctCatDispSeq")]
        public int DispaySeq { get; set; }
    }

    public class ProductType
    {
        [JsonProperty(PropertyName = "productTypeMasterId")]
        public int ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "productCode")]
        public string ProductTypeCode { get; set; }

        [JsonProperty(PropertyName = "productTypeName")]
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
        public int CountryId { get; set; }

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
        public int StoreId { get; set; }

        [JsonProperty(PropertyName = "orgId")]
        public int OrgId { get; set; }

        [JsonProperty(PropertyName = "storeCode")]
        public string StoreCode { get; set; }

        [JsonProperty(PropertyName = "storeName")]
        public string StoreName { get; set; }
    }
}
