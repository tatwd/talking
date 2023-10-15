using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Talking.Domain.Data
{
    public class TalkingDbContext
    {
        private readonly IMongoDatabase _context ;

        public TalkingDbContext(IOptions<MongoSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _context = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<Comment> Comments
        {
            get { return _context.GetCollection<Comment>("comments"); }
        }

        public IMongoCollection<User> Users
        {
            get { return _context.GetCollection<User>("users"); }
        }
    }
}
