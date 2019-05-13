using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Talking.Api.Data
{
    public class User
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("ip_v4")]
        public string IPv4 { get; set; }
    }
}
