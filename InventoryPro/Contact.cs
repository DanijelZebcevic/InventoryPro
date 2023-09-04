using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace InventoryPro
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

    }
}
