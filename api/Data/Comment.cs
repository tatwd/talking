using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Talking.Api.Data
{
  public class Comment
    {
        [BsonId]
        public ObjectId ID { get; set; }

        [BsonElement("owner")]
        public User Owner { get; set; }

        [BsonElement("post_title")]
        public string  PostTitle { get; set; }

        [BsonElement("post_url")]
        public string  PostUrl { get; set; }

        [BsonElement("html_text")]
        public string HtmlText { get; set; }

        [BsonElement("utc_created")]
        public DateTime UtcCreated { get; set; } = DateTime.UtcNow;
    }
}
