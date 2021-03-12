using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Filters
{
    public class RestaurantFilter
    {
        public int Limit { get; set; }
        public string LastId { get; set; }
        public string Name { get; set; }

        public RestaurantFilter()
        {
            Name = "";
            LastId = "";
            Limit = 10;
        }
    }
}
