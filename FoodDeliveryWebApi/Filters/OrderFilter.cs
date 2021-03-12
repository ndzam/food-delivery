using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Filters
{
    public class OrderFilter
    {
        public int Limit { get; set; }
        public string LastId { get; set; }

        public OrderFilter()
        {
            LastId = "";
            Limit = 10;
        }
    }
}
