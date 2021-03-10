using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryWebApi.Models
{
    [FirestoreData]
    public class Block
    {
        public string Id { get; set; }
        [FirestoreProperty]
        public string UserId { get; set; }
    }
}
