using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Models
{
    public class StatusHistoryItem
    {
        public string Status { get; set; }
        public int Date { get; set; }
    }
}
