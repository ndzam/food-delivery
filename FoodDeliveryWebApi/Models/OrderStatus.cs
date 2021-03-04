using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Models
{
    public class OrderStatus
    {
        public string CurrentStatus { get; set; }
        public List<StatusHistoryItem> StatusHistory { get; set; }
    }
}
