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

        private static Dictionary<string, int> statuses = new Dictionary<string, int>
        {
            { PLACED, 0 },
            { CANCELED, 0},
            { PROCESSING, 1},
            { ROUTE, 2 },
            { DELIVERED, 3},
            { RECIEVED, 4 }
        };

        private static Dictionary<string, string> permisions = new Dictionary<string, string>
        {
            { PLACED, UserRoles.USER },
            { CANCELED, UserRoles.USER},
            { PROCESSING, UserRoles.OWNER},
            { ROUTE, UserRoles.OWNER },
            { DELIVERED, UserRoles.OWNER},
            { RECIEVED, UserRoles.USER }
        };

        public static bool isValidStatusChange(string current, string next, string role)
        {
            if (!permisions.ContainsKey(next) || !permisions.ContainsKey(current)) return false;
            if (current.Equals(next)) return false;
            if (current.Equals(CANCELED)) return false;
            int k = statuses[next] - statuses[current];
            if (k != 0 && k != 1) return false;
            if (!permisions[next].Equals(role)) return false;
            return true;
        }
    }
}
