using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Constants
{
    public static class OrderStatuses
    {
        public const string PLACED = "placed";
        public const string CANCELED = "canceled";
        public const string PROCESSING = "processing";
        public const string ROUTE = "route";
        public const string DELIVERED = "delivered";
        public const string RECIEVED = "received";
    }
}
