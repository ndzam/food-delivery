using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Requests
{
    public class RestaurantPostRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
