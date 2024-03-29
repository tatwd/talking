using System;
using Talking.Domain.Data;

namespace Talking.Api.Models
{
    public class CommentDto
    {
        public string ID { get; set; }
        public object Owner { get; set; }
        public string  PostTitle { get; set; }
        public string  PostUrl { get; set; }
        public string HtmlText { get; set; }
        public DateTime UtcCreated { get; set; }

        public CommentDto(Comment comment)
        {
            ID = comment.Id.ToString();
            Owner = new {
                Name = comment.Owner.Name,
                Url = comment.Owner.Url
            };
            PostTitle = comment.PostTitle;
            PostUrl = comment.PostUrl;
            HtmlText = comment.HtmlText;
            UtcCreated = comment.UtcCreated;
        }
    }
}
