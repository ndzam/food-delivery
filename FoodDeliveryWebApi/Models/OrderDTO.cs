using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Models
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RestaurantOwnerId { get; set; }
        public string RestaurantName { get; set; } 
        public string RestaurantId { get; set; }
        public List<OrderItemDTO> Items { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public int Date { get; set; }
        public bool IsRestaurantDeleted { get; set; }
        public bool IsUserBlocked { get; set; }
    }
}
