using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string RestaurantOwnerId { get; set; }
        public string RestaurantId { get; set; }
        public List<OrderItem> Items { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public int Date { get; set; }
    }

    
}
