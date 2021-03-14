using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Models
{
    public class OrderItem
    {
        public string FoodId { get; set; }
        public int? Quantity { get; set; }
    }
}
