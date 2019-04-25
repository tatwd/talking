using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Talking.Api.Data;

namespace Talking.Api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TalkingDbContext _context = null;

        public CommentRepository(IOptions<MongoSettings> settings)
        {
            _context = new TalkingDbContext(settings);
        }

        public IList<Comment> GetComments()
        {
            var data = _context.Comments.Find(_ => true).ToList();
            return data;
        }

        public void InsertComment(Comment comment)
        {
            _context.Comments.
            _context.Comments.InsertOne(comment);
        }
    }
}
