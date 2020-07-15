using System;
using System.ComponentModel;
using Cognizant.Hackathon.Mobile.Core.Models;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    [Serializable]
    public class Product : ModelBase, INotifyPropertyChanged
    {
        private int _quantity;

        [JsonProperty(PropertyName = "ProductType")]
        public string ProductType { get; set; }

        [JsonProperty(PropertyName = "ProductName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "Quantity")]
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "IsNewProductName")]
        public bool IsNewProductName { get; set; }

        [JsonProperty(PropertyName = "ProductId")]
        public string ProductId { get; set; }

        [JsonProperty(PropertyName = "ProductTypeId")]
        public string ProductTypeId { get; set; }

        [JsonProperty(PropertyName = "ProductNameId")]
        public string ProductNameId { get; set; }

        public Product ShallowCopy()
        {
            return (Product)this.MemberwiseClone();
        }
    }
}
