using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro
{
    public class Delivery
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        public string Deliverer { get; set; }

        public DateTime DeliveryDate { get; set; }

        public List<Items> DeliveredItems { get; set; }

        public string DeliveryStatus { get; set; }

    }
}
