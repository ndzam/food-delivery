using FoodDeliveryWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Requests
{
    public class OrderPostRequest
    {
        public string RestaurantId { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
