using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Talking.Api.Data
{
  public class Comment
    {
        [BsonId]
        [BsonElement("id")]
        public ObjectId ID { get; set; }

        [BsonElement("user_id")]
        public ObjectId UserID { get; set; }

        [BsonElement("post_url")]
        public string  PostUrl { get; set; }

        [BsonElement("html_text")]
        public string HtmlText { get; set; }

        [BsonElement("utc_created")]
        public DateTime UtcCreated { get; set; } = DateTime.UtcNow;
    }
}
