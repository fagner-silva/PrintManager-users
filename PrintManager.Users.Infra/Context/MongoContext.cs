using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PrintManager.Users.Domain.Entities;
using PrintManager.Users.Infra.Settings;

namespace PrintManager.Users.Infra.Context
{
    public class MongoContext
    {
        public IMongoCollection<User> Users { get; }

        public MongoContext(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);

            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);

            Users = database.GetCollection<User>(settings.Value.UsersCollectionName);
        }
    }
}
