using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryPro
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Customer { get; set; }

        public DateTime DeliveryDate { get; set; }

        public List<Items> OrderedItems { get; set; }

        public bool OrderIsDelivered { get; set; }
    }
}
