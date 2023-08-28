

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace InventoryPro
{
    public class Bill
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<Items> Items { get; set; }

        public string Buyer { get; set; }

        public DateTime DateOfPurchase { get; set; }

        public float Sum { get; set; }
    }
}
