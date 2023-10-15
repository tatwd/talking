using System.Collections.Generic;
using MongoDB.Driver;
using Talking.Domain.Data;

namespace Talking.Domain.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TalkingDbContext _context;

        public CommentRepository(TalkingDbContext context)
        {
            _context = context;
        }

        public long GetCount(string postUrl)
        {
            var filter = Builders<Comment>.Filter.Eq(_ => _.PostUrl, postUrl);
            return _context.Comments.CountDocuments(filter);
        }

        public IList<Comment> GetComments(int limit)
        {
            var data = _context.Comments
                .Find(_ => true)
                .Limit(limit)
                .SortByDescending(_ => _.UtcCreated)
                .ToList();
            return data;
        }

        public IList<Comment> GetComments(string postUrl)
        {
            var data = _context.Comments
                .Find(_ => _.PostUrl == postUrl)
                .SortByDescending(_ => _.UtcCreated)
                .ToList();
            return data;
        }

        public IList<Comment> GetComments(string postUrl, int page, int limit)
        {
            var data = _context.Comments
                .Find(_ => _.PostUrl == postUrl)
                .Skip((page - 1) * limit)
                .Limit(limit)
                .SortByDescending(_ => _.UtcCreated)
                .ToList();
            return data;
        }

        public void InsertComment(Comment comment)
        {
            _context.Comments.InsertOne(comment);
        }
    }
}
