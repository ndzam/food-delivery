using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Requests
{
    public class UserPutRequest
    {
        public string Role { get; set; }
        public string Name { get; set; }
    }
}
