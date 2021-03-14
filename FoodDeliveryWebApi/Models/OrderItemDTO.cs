using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Models
{
    public class OrderItemDTO
    {
        public string FoodId { get; set; }
        public string FoodName { get; set; }
        public double Price { get; set; }
        public int? Quantity { get; set; }
    }
}
