using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class Order
    {
        [JsonProperty(PropertyName = "orderId")]
        public int OrderId { get; set; }

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
        public bool isSelfPickup { get; set; }

        [JsonProperty(PropertyName = "isVolunteered")]
        public bool isVolunteered { get; set; }

        [JsonProperty(PropertyName = "volunteerId")]
        public int? VolunteerId { get; set; }

        [JsonProperty(PropertyName = "volunteer")]
        public string Volunteer { get; set; }

        [JsonProperty(PropertyName = "orderTotal")]
        public double OrderTotal { get; set; }

        [JsonProperty(PropertyName = "products")]
        public List<Product> Products { get; set; }

        public string Description
        {
            get
            {
                string prdDesc = string.Empty;
                if(Products !=null && Products.Count > 0)
                {
                    foreach (var item in Products)
                    {
                        prdDesc += item.Description +";";
                    }
                    
                }
                return prdDesc;
            }
        }
    }
}
