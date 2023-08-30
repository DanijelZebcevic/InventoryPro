using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;


namespace InventoryPro
{
    public class Delivery
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Deliverer { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public List<Items> DeliveredItems { get; set; }

        public bool OrderIsDelivered { get; set; }

    }
}
