using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Talking.Domain.Data
{
    public class TalkingDbContext
    {
        private readonly IMongoDatabase _context ;

        public TalkingDbContext(IConfiguration configuration, ILogger<TalkingDbContext> logger)
        {
            var mongoConnStr = Environment.GetEnvironmentVariable("MONGO_URL") ??
                               configuration["MongoSettings:ConnectionUrl"];
            var dbName = Environment.GetEnvironmentVariable("MONGO_DB") ??
                         configuration["MongoSettings:DatabaseName"];

            logger.LogDebug("mongoConnStr: {MongoConnStr} dbName: {DbName}", mongoConnStr, dbName);

            var client = new MongoClient(mongoConnStr);
            _context = client.GetDatabase(dbName);
        }

        public IMongoCollection<Comment> Comments => _context.GetCollection<Comment>("comments");

        public IMongoCollection<User> Users => _context.GetCollection<User>("users");
    }
}
