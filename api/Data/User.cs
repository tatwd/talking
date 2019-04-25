using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Talking.Api.Data
{
    public class User
    {
        [BsonId]
        [BsonElement("id")]
        public ObjectId ID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("ip_v4")]
        public string IPv4 { get; set; }

        [BsonElement("is_admin")]
        public bool IsAdmin { get; set; } = false;
    }
}
